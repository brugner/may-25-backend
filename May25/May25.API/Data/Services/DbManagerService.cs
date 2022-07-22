using Bogus;
using May25.API.Core.Constants;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Helpers;
using May25.API.Core.Models.Entities;
using May25.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace May25.API.Data.Services
{
    public class DbManagerService : IDbManagerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DbManagerService> _logger;

        public DbManagerService(IServiceScopeFactory scopeFactory, ILogger<DbManagerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            Randomizer.Seed = new Random(36535161);
        }

        public void Migrate()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            foreach (var item in context.Database.GetPendingMigrations())
            {
                _logger.LogInformation($"Applying pending migration: {item}");
            }

            context.Database.Migrate();
        }

        public void Seed()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            SeedRoles(context);
            SeedUsers(context);
            SeedPlaces(context);
            SeedMakes(context);
            SeedCars(context);
            SeedTrips(context);
        }

        private void SeedRoles(AppDbContext context)
        {
            if (!context.Roles.Any())
            {
                _logger.LogInformation("Seeding roles..");

                var roles = GetRolesSeeds();

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }

        private void SeedUsers(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                _logger.LogInformation("Seeding users..");

                var users = GetUsersSeeds(context.Roles);

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }

        private void SeedPlaces(AppDbContext context)
        {
            if (!context.Places.Any())
            {
                _logger.LogInformation("Seeding places..");

                var places = GetPlacesSeeds();

                context.Places.AddRange(places);
                context.SaveChanges();
            }
        }

        private void SeedMakes(AppDbContext context)
        {
            if (!context.Makes.Any())
            {
                _logger.LogInformation("Seeding makes..");

                var makes = GetMakesSeeds();

                context.Makes.AddRange(makes);
                context.SaveChanges();
            }
        }

        private void SeedCars(AppDbContext context)
        {
            if (!context.Cars.Any())
            {
                _logger.LogInformation("Seeding cars..");

                var cars = GetCarsSeeds(context.Users, context.Makes, context.Models);

                context.Cars.AddRange(cars);
                context.SaveChanges();
            }
        }

        private void SeedTrips(AppDbContext context)
        {
            if (!context.Trips.Any())
            {
                _logger.LogInformation("Seeding trips..");

                var trips = GetTripsSeeds(context.Users, context.Cars, context.Places);

                context.Trips.AddRange(trips);
                context.SaveChanges();
            }
        }

        private IEnumerable<Role> GetRolesSeeds()
        {
            return new Role[]
            {
                new Role {  Name = "user" },
                new Role {  Name = "admin" },
                new Role {  Name = "apiclient" }
            };
        }

        private IEnumerable<User> GetUsersSeeds(DbSet<Role> roles)
        {
            string passwordHash = "1000:KBhgbN2BijErjIAi5RPbnhsjHZcJWYhi:bPJsZtaGaCOWq+twwCbl2oG8iz8=";
            var genders = new string[] { "F", "M", "O", "N" };
            var preValues = new byte[] { 1, 5, 10 };
            string firstName = string.Empty;
            string lastName = string.Empty;

            var seeds = new Faker<User>()
                .RuleFor(x => x.PasswordHash, passwordHash)
                .RuleFor(x => x.CreatedAt, faker => faker.Date.PastOffset(2))
                .RuleFor(x => x.FirstName, faker =>
                {
                    firstName = faker.Name.FirstName();
                    return firstName;
                })
                .RuleFor(x => x.LastName, faker =>
                {
                    lastName = faker.Name.LastName();
                    return lastName;
                })
                .RuleFor(x => x.Email, faker => $"{firstName}.{lastName}@may25.com".ToLower())
                .RuleFor(x => x.Roles, f => new List<UserRoles> { new UserRoles { Role = roles.Single(x => x.Name.Equals(AppRoles.User)) } })
                .RuleFor(x => x.Birthday, faker => faker.Date.Between(new DateTime(1980, 1, 1), new DateTime(2000, 12, 31)))
                .RuleFor(x => x.Gender, faker => faker.PickRandom(genders))
                .RuleFor(x => x.Bio, faker => faker.Random.Words(15))
                .RuleFor(x => x.Talk, faker => faker.PickRandom(preValues))
                .RuleFor(x => x.Music, faker => faker.PickRandom(preValues))
                .RuleFor(x => x.Pets, faker => faker.PickRandom(preValues))
                .RuleFor(x => x.Smoking, faker => faker.PickRandom(preValues))
                .RuleFor(x => x.IsEmailConfirmed, faker => false)
                .RuleFor(x => x.EmailConfirmationToken, faker => CryptoHelper.GenerateRandomString(length: 40))
                .RuleFor(x => x.IsSubscribed, faker => true)
                .Generate(100);

            var adminUser = new User
            {
                Email = "admin1@may25.com",
                PasswordHash = "1000:KBhgbN2BijErjIAi5RPbnhsjHZcJWYhi:bPJsZtaGaCOWq+twwCbl2oG8iz8=",
                Roles = new List<UserRoles> { new UserRoles { Role = roles.Single(x => x.Name.Equals(AppRoles.Admin)) } },
                FirstName = "Maximum",
                LastName = "Sudo",
                Birthday = new DateTime(1992, 9, 7),
                Gender = "O",
                Bio = "GOD MODE ACTIVATEDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD",
                CreatedAt = DateTime.Now,
                IsEmailConfirmed = false,
                EmailConfirmationToken = CryptoHelper.GenerateRandomString(length: 40),
                IsSubscribed = true
            };

            return seeds.Append(adminUser);
        }

        private IEnumerable<Place> GetPlacesSeeds()
        {
            return new Place[]
            {
                new Place
                {
                    GooglePlaceId = "ChIJCzEVa02rt5URBcI5vkQMWAI",
                    FormattedAddress = "199,, Av. Francia Bis 175, Rosario, Santa Fe, Argentina",
                    Lat = -32.9252215,
                    Lng = -60.66147410000001,
                    ValidUntil = DateTime.Now.AddDays(30)
                },
                new Place
                {
                    GooglePlaceId = "ChIJd-vezpeiMpQRoF2HV3I4id0",
                    FormattedAddress = "Nudo Vial Mitre, Córdoba, Argentina",
                    Lat = -31.4228144,
                    Lng = -64.1726106,
                    ValidUntil = DateTime.Now.AddDays(30)
                },
                new Place
                {
                    GooglePlaceId = "ChIJP4011HGrt5URaVOxGj8377c",
                    FormattedAddress = "Parque Independencia, S2000 Rosario, Santa Fe, Argentina",
                    Lat = -32.9583697,
                    Lng = -60.6591846,
                    ValidUntil = DateTime.Now.AddDays(30)
                },
                new Place
                {
                    GooglePlaceId = "ChIJN6i2hZSiMpQRidU-PUn3T9s",
                    FormattedAddress = "Sarmiento s/n, X5000 Córdoba, Argentina",
                    Lat = -31.4299763,
                    Lng = -64.18046490000002,
                    ValidUntil = DateTime.Now.AddDays(30)
                }
            };
        }

        private IEnumerable<Make> GetMakesSeeds()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Seeds", "makes-and-models-seeds.txt");
            var fileText = File.ReadAllText(filePath);
            var groups = fileText.Split(Environment.NewLine + Environment.NewLine);
            var result = new List<Make>();

            foreach (var group in groups)
            {
                var items = group.Split(Environment.NewLine);

                if (items.Length == 1)
                    continue;

                var make = new Make
                {
                    Name = items.First(),
                    Models = new List<Model>()
                };

                for (int i = 1; i < items.Length; i++)
                {
                    make.Models.Add(new Model { Name = items[i] });
                }

                result.Add(make);
            }

            return result.OrderBy(x => x.Name);
        }

        private IEnumerable<Car> GetCarsSeeds(DbSet<User> users, DbSet<Make> makes, DbSet<Model> models)
        {
            var makeId = 0;

            var cars = new Faker<Car>()
                .RuleFor(x => x.DriverId, faker => faker.PickRandom(users.Select(x => x.Id).ToArray()))
                .RuleFor(x => x.MakeId, faker =>
                {
                    makeId = faker.PickRandom(makes.Select(x => x.Id).ToArray());
                    return makeId;
                })
                .RuleFor(x => x.ModelId, faker => faker.PickRandom(models.Where(x => x.MakeId == makeId).Select(x => x.Id).ToArray()))
                .RuleFor(x => x.PlateNumber, faker => faker.Random.AlphaNumeric(7).ToUpper())
                .RuleFor(x => x.Color, faker => (byte)faker.Random.Number(1, 15))
                .RuleFor(x => x.Year, faker => (short)faker.Date.Between(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1)).Year)
                .Generate(5);

            return cars;
        }

        private IEnumerable<Trip> GetTripsSeeds(DbSet<User> users, DbSet<Car> cars, DbSet<Place> places)
        {
            var possibleDrivers = users.Where(x => x.Cars.Count > 0).ToArray();
            var driverId = 0;
            var originId = 0;

            var seeds = new Faker<Trip>()
                  .RuleFor(x => x.DriverId, faker =>
                  {
                      driverId = faker.PickRandom(possibleDrivers).Id;
                      return driverId;
                  })
                  .RuleFor(x => x.CarId, faker =>
                    {
                        var possibleCars = possibleDrivers.First(x => x.Id == driverId).Cars;
                        return faker.PickRandom(possibleCars).Id;
                    })
                  .RuleFor(x => x.OriginId, faker =>
                  {
                      originId = faker.PickRandom(places.ToArray()).Id;
                      return originId;
                  })
                  .RuleFor(x => x.DestinationId, faker =>
                  {
                      var possibleDestinations = places.Where(x => x.Id != originId).ToArray();
                      return faker.PickRandom(possibleDestinations).Id;
                  })
                  .RuleFor(x => x.Departure, faker => faker.Date.SoonOffset(10, DateTimeOffset.Now.AddDays(1)))
                  .RuleFor(x => x.MaxPassengers, faker => faker.PickRandom(new byte[] { 1, 2, 3, 4 }))
                  .RuleFor(x => x.Description, faker => faker.Lorem.Sentence(10, 5))
                  .RuleFor(x => x.Description, faker => faker.Lorem.Sentence(10, 5))
                  .RuleFor(x => x.Distance, faker => faker.Random.Number(10000, 500000))
                  .RuleFor(x => x.Duration, faker => faker.Random.Number(3600, 54000))
                  .RuleFor(x => x.SuggestedCost, faker => faker.Random.Number(1000, 5000))
                  .RuleFor(x => x.Cost, faker => faker.Random.Number(1000, 5000))
                  .RuleFor(x => x.CostPerPassenger, faker => faker.Random.Number(300, 1000))
                  .RuleFor(x => x.CreatedAt, faker => faker.Date.RecentOffset(3))
                  .Generate(30);

            return seeds;
        }
    }
}
