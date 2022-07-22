using AutoMapper;
using GoogleApi;
using GoogleApi.Entities.Common.Enums;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using May25.API.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GoogleMapsOptions _googleMapsOptions;
        private readonly ILogger<PlaceService> _logger;

        public PlaceService(IUnitOfWork unitOfWork, IMapper mapper,
            IOptions<GoogleMapsOptions> googleMapsOptions, ILogger<PlaceService> logger)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _googleMapsOptions = googleMapsOptions.Value.ThrowIfNull(nameof(googleMapsOptions));
            _logger = logger.ThrowIfNull(nameof(logger));
        }

        public async Task<IEnumerable<PlaceDTO>> GetAllAsync()
        {
            var places = await _unitOfWork.Places.GetAllAsync();

            return _mapper.Map<IEnumerable<PlaceDTO>>(places);
        }

        public async Task<PlaceDetailDTO> GetPlaceDetailsAsync(string id)
        {
            var req = new GoogleApi.Entities.Places.Details.Request.PlacesDetailsRequest
            {
                Key = _googleMapsOptions.APIKey,
                PlaceId = id,
                Language = Language.Spanish
            };

            var response = await GooglePlaces.Details.QueryAsync(req);

            if (response.Status == Status.Ok)
            {
                return _mapper.Map<PlaceDetailDTO>(response.Result);
            }
            else
            {
                _logger.LogError($"Google Places Details error: {response.ErrorMessage}");
                return null;
            }
        }

        public async Task<IEnumerable<PlaceAutocompleteDTO>> QueryAutocompleteAsync(string searchTerm)
        {
            var req = new GoogleApi.Entities.Places.AutoComplete.Request.PlacesAutoCompleteRequest
            {
                Key = _googleMapsOptions.APIKey,
                Input = searchTerm,
                Language = Language.Spanish,
                Components = new List<KeyValuePair<Component, string>>
                {
                    new KeyValuePair<Component, string>(Component.Country, _googleMapsOptions.Country)
                }
            };

            var response = await GooglePlaces.AutoComplete.QueryAsync(req);

            if (response.Status == Status.Ok)
            {
                return _mapper.Map<IEnumerable<PlaceAutocompleteDTO>>(response.Predictions);
            }
            else
            {
                _logger.LogError($"Google Places Autocomplete error: {response.ErrorMessage}");
                return null;
            }
        }

        public async Task<IEnumerable<PlaceAutocompleteDTO>> SearchNearbyAsync(double latitude, double longitude)
        {
            var req = new GoogleApi.Entities.Maps.Geocoding.Location.Request.LocationGeocodeRequest
            {
                Key = _googleMapsOptions.APIKey,
                Location = new GoogleApi.Entities.Common.Location(latitude, longitude),
                Language = Language.Spanish
            };

            var response = await GoogleMaps.LocationGeocode.QueryAsync(req);

            if (response.Status == Status.Ok)
            {
                return _mapper.Map<IEnumerable<PlaceAutocompleteDTO>>(response.Results);
            }
            else
            {
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    _logger.LogError($"Google Places Search Near By error: {response.ErrorMessage}");
                }

                return null;
            }
        }

        public async Task UpdatePlaceCacheAsync(Place place)
        {
            if (place.ValidUntil.HasValue)
            {
                return;
            }

            var placeDetails = await GetPlaceDetailsAsync(place.GooglePlaceId);

            place.FormattedAddress = placeDetails.Description;
            place.Lat = placeDetails.Lat;
            place.Lng = placeDetails.Lng;
            place.ValidUntil = DateTime.Now.AddDays(30);

            await _unitOfWork.CompleteAsync();
        }
    }
}
