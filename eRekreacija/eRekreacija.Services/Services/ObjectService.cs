using AutoMapper;
using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.Context;
using eRekreacija.Services.Database.Entities;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace eRekreacija.Services.Services
{
    public class ObjectService : BaseCRUDService<tbl_Objects, ObjectsDTO, ObjectInsertRequest, ObjectUpdateRequest>, IObjectService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _host;

        public ObjectService(RekreacijaContext rekreacijaContext, IMapper mapper, UserManager<ApplicationUser> userManager, IHostingEnvironment host) : base(rekreacijaContext, mapper)
        {
            _host = host;
            _userManager = userManager;
        }

        public override async Task BeforeImageInsert(tbl_Objects entity, ObjectInsertRequest insert)
        {
            string cleanedName = string.IsNullOrWhiteSpace(insert.name)
                   ? "defaultobject"
                   : new string(insert.name.Where(char.IsLetterOrDigit).ToArray()).ToLower();

            string fileName = $"{cleanedName}.jpg";
            string filePath = Path.Combine(_host.WebRootPath, "images", fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, insert.ObjectImage);
            entity.ImagePath = $"/images/{fileName}";

            await base.BeforeImageInsert(entity, insert);
        }
        public override async Task BeforeImageUpdate(tbl_Objects entity, ObjectUpdateRequest update)
        {
            if (update.ObjectImage != null)
            {
                string imagePath = Path.Combine(_host.WebRootPath, entity.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

                string cleanedName = string.IsNullOrWhiteSpace(update.name)
               ? "defaultobject"
               : new string(update.name.Where(char.IsLetterOrDigit).ToArray()).ToLower();

                string fileName = $"{cleanedName}.jpg";
                string filePath = Path.Combine(_host.WebRootPath, "images", fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, update.ObjectImage);
                entity.ImagePath = $"/images/{fileName}";

                await base.BeforeImageUpdate(entity, update);
            }
            else
                await base.BeforeImageUpdate(entity, update);
        }
        public override async Task BeforeInsert(tbl_Objects entity, ObjectInsertRequest insert)
        {
            if (insert.sportId != null && insert.sportId.Any())
            {
                foreach (var id in insert.sportId)
                {
                    var objectSportCategory = new tbl_ObjectSportCategory
                    {
                        object_id = entity.id,
                        sport_category_id = id
                    };
                    await _rekreacijaContext.Set<tbl_ObjectSportCategory>().AddAsync(objectSportCategory);
                }

                await _rekreacijaContext.SaveChangesAsync();
            }
        }
        public override async Task BeforeUpdate(tbl_Objects entity, ObjectUpdateRequest update)
        {
            var findSportsId = _rekreacijaContext.Set<tbl_ObjectSportCategory>().Where(s => s.object_id == entity.id).ToList();
            if (findSportsId.Count() != 0)
            {
                _rekreacijaContext.RemoveRange(findSportsId);

                if (update.sportId != null && update.sportId.Any())
                {
                    foreach (var id in update.sportId)
                    {
                        var objectSportCategory = new tbl_ObjectSportCategory
                        {
                            object_id = entity.id,
                            sport_category_id = id
                        };
                        await _rekreacijaContext.Set<tbl_ObjectSportCategory>().AddAsync(objectSportCategory);
                    }

                    await _rekreacijaContext.SaveChangesAsync();
                }
            }
            else
            {
                if (update.sportId != null && update.sportId.Any())
                {
                    foreach (var id in update.sportId)
                    {
                        var objectSportCategory = new tbl_ObjectSportCategory
                        {
                            object_id = entity.id,
                            sport_category_id = id
                        };
                        await _rekreacijaContext.Set<tbl_ObjectSportCategory>().AddAsync(objectSportCategory);
                    }

                    await _rekreacijaContext.SaveChangesAsync();
                }
            }
            await base.BeforeUpdate(entity, update);
        }
        public override async Task BeforeDelete(tbl_Objects db)
        {
            string imagePath = Path.Combine(_host.WebRootPath, db.ImagePath.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            await base.BeforeDelete(db);
        }
        public async Task<List<ObjectsDTO>> GetAllObjectsOfUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId);

            var objects = await _rekreacijaContext.TblObject.Where(s => s.user_id == userId).OrderByDescending(s => s.created_date).Include(s => s.ObjectSportCategory).ToListAsync();

            var objectDTO = objects.Select(obj => new ObjectsDTO
            {
                id = obj.id,
                name = obj.name,
                address = obj.address,
                city = obj.city,
                description = obj.description,
                ImagePath = obj.ImagePath,
                price = obj.price,
                sportsId = obj.ObjectSportCategory.Select(s => s.sport_category_id).ToList(),
            }).ToList();
            return objectDTO;
        }

        public async Task<List<ObjectsDTO>> GetObjectByCategory(string userId, int categoryId, string? name = null)
        {
            var objectsIds = await _rekreacijaContext.Set<tbl_ObjectSportCategory>().Where(s => s.sport_category_id == categoryId).Select(s => s.object_id).ToListAsync();

            var userFavorites = await _rekreacijaContext.Set<tbl_Favorites>().Where(f => f.user_id == userId).Select(f => f.object_id).ToListAsync();

            var objectsDTO = await _rekreacijaContext.Set<tbl_Objects>()
                               .Where(s => objectsIds.Contains(s.id) && (string.IsNullOrEmpty(name) || s.name.Contains(name)))

                .Select(obj => new ObjectsDTO
                {
                    id = obj.id,
                    name = obj.name,
                    address = obj.address,
                    city = obj.city,
                    description = obj.description,
                    ImagePath = obj.ImagePath,
                    price = obj.price,
                    rating = obj.Reviews.Any() ? obj.Reviews.Average(r => r.rating) : 0,
                    isFavorites = userFavorites.Contains(obj.id)
                }).ToListAsync();

            return objectsDTO;
        }

        public async Task<List<ObjectsDTO>> GetFavoritesObject(string userId)
        {
            var objectId = await _rekreacijaContext.Set<tbl_Favorites>().Where(s => s.user_id == userId).Select(s => s.object_id).ToListAsync();

            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(s => objectId.Contains(s.id)).Include(s => s.Reviews).ToListAsync();

            var objectsDTO = await _rekreacijaContext.Set<tbl_Objects>()
                .Where(s => objectId.Contains(s.id))
            .Include(s => s.ObjectSportCategory)
            .Select(obj => new ObjectsDTO
            {
                id = obj.id,
                name = obj.name,
                address = obj.address,
                city = obj.city,
                description = obj.description,
                ImagePath = obj.ImagePath,
                price = obj.price,
                rating = obj.Reviews.Any() ? obj.Reviews.Average(r => r.rating) : 0,
                sportsId = obj.ObjectSportCategory.Select(s => s.sport_category_id).ToList(),
            }).ToListAsync();

            return objectsDTO;
        }

        public override async Task<List<ObjectsDTO>> Get()
        {
            var objectsDTO = await _rekreacijaContext.Set<tbl_Objects>()
               .Select(obj => new ObjectsDTO
               {
                   id = obj.id,
                   name = obj.name,
                   address = obj.address,
                   city = obj.city,
                   description = obj.description,
                   ImagePath = obj.ImagePath,
                   price = obj.price,
                   rating = obj.Reviews.Any() ? obj.Reviews.Average(r => r.rating) : 0,
               }).ToListAsync();

            return objectsDTO;
        }

        static MLContext mlContext = new MLContext();
        static object isLocked = new object();
        static ITransformer model = null;
        static Dictionary<string, uint> userGuidToInt = new();
        static Dictionary<int, uint> objectIdToUint = new();

        public List<ObjectsDTO> Recomended(string userId)
        {
            lock (isLocked)
            {
                var reviews = _rekreacijaContext.Set<tbl_Review>().ToList();
                var appointments = _rekreacijaContext.Set<tbl_Appointment>().ToList();
                var favorites = _rekreacijaContext.Set<tbl_Favorites>().ToList();

                if (reviews.Count < 10)
                {
                    var popularObjects = _rekreacijaContext.Set<tbl_Objects>()
                        .OrderByDescending(o => o.Reviews.Count)
                        .Take(4)
                        .ToList();

                    if (popularObjects.Count == 0)
                    {
                        popularObjects = _rekreacijaContext.Set<tbl_Objects>()
                            .OrderBy(o => Guid.NewGuid())
                            .Take(4)
                            .ToList();
                    }

                    return _mapper.Map<List<ObjectsDTO>>(popularObjects);
                }

                uint userCounter = 1;
                uint objectCounter = 1;

                foreach (var user in reviews.Select(r => r.user_id).Distinct())
                {
                    if (!userGuidToInt.ContainsKey(user))
                        userGuidToInt[user] = userCounter++;
                }

                foreach (var obj in reviews.Select(r => r.object_id).Distinct())
                {
                    if (!objectIdToUint.ContainsKey(obj))
                        objectIdToUint[obj] = objectCounter++;
                }

                var data = reviews.Select(r => new ProductEntry
                {
                    UserID = userGuidToInt[r.user_id],
                    ObjectID = objectIdToUint[r.object_id],
                    label = 1
                }).ToList();

                foreach (var app in appointments)
                {
                    if (!userGuidToInt.ContainsKey(app.user_id))
                        userGuidToInt[app.user_id] = userCounter++;

                    if (!objectIdToUint.ContainsKey(app.object_id))
                        objectIdToUint[app.object_id] = objectCounter++;

                    data.Add(new ProductEntry
                    {
                        UserID = userGuidToInt[app.user_id],
                        ObjectID = objectIdToUint[app.object_id],
                        label = 1
                    });
                }

                foreach (var fav in favorites)
                {
                    if (!userGuidToInt.ContainsKey(fav.user_id))
                        userGuidToInt[fav.user_id] = userCounter++;

                    if (!objectIdToUint.ContainsKey(fav.object_id))
                        objectIdToUint[fav.object_id] = objectCounter++;

                    data.Add(new ProductEntry
                    {
                        UserID = userGuidToInt[fav.user_id],
                        ObjectID = objectIdToUint[fav.object_id],
                        label = 1
                    });
                }

                if (model == null)
                {
                    var trainData = mlContext.Data.LoadFromEnumerable(data);
                    var options = new MatrixFactorizationTrainer.Options
                    {
                        MatrixColumnIndexColumnName = nameof(ProductEntry.UserID),
                        MatrixRowIndexColumnName = nameof(ProductEntry.ObjectID),
                        LabelColumnName = nameof(ProductEntry.label),
                        LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass,
                        Alpha = 0.01,
                        Lambda = 0.025,
                        NumberOfIterations = 100
                    };

                    var estimator = mlContext.Recommendation().Trainers.MatrixFactorization(options);
                    model = estimator.Fit(trainData);
                }

                if (!userGuidToInt.ContainsKey(userId))
                {
                    var fallbackObjects = _rekreacijaContext.Set<tbl_Objects>()
                        .OrderByDescending(o => o.Reviews.Count)
                        .Take(4)
                        .ToList();

                    return _mapper.Map<List<ObjectsDTO>>(fallbackObjects);
                }

                uint mappedUserId = userGuidToInt[userId];

                var allObjects = _rekreacijaContext.Set<tbl_Objects>().ToList();

                var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductEntry, Copurchase_prediction>(model);

                var recommendations = allObjects
                    .Where(o => objectIdToUint.ContainsKey(o.id))
                    .Select(o => new
                    {
                        Object = o,
                        Score = predictionEngine.Predict(new ProductEntry
                        {
                            UserID = mappedUserId,
                            ObjectID = objectIdToUint[o.id]
                        }).Score
                    })
                    .OrderByDescending(x => x.Score)
                    .Take(5)
                    .Select(x => new ObjectsDTO
                    {
                        id = x.Object.id,
                        name = x.Object.name,
                        address = x.Object.address,
                        city = x.Object.city,
                        description = x.Object.description,
                        ImagePath = x.Object.ImagePath,
                        price = x.Object.price,
                        rating = x.Object.Reviews.Any() ? x.Object.Reviews.Average(r => r.rating) : 0
                    })
                    .ToList();

                return recommendations;
            }
        }

        public class Copurchase_prediction
        {
            public float Score { get; set; }
        }
        public class ProductEntry
        {
            [KeyType(count: 100)]
            public uint UserID { get; set; }
            [KeyType(count: 100)]
            public uint ObjectID { get; set; }
            public float label { get; set; }
        }

        public async Task<List<ObjectsDTO>> GetRecentAppointments(string userId)
        {
            var objectIds = await _rekreacijaContext.Set<tbl_Appointment>().Where(s => s.user_id == userId).OrderByDescending(s => s.appointment_date).Select(s => s.object_id).Distinct().ToListAsync();

            var objects = await _rekreacijaContext.Set<tbl_Objects>().Where(s => objectIds.Contains(s.id)).Select(s => new ObjectsDTO
            {
                id = s.id,
                name = s.name,
                address = s.address,
                city = s.city,
                description = s.description,
                ImagePath = s.ImagePath,
                price = s.price,
                rating = s.Reviews.Any() ? s.Reviews.Average(r => r.rating) : 0
            }).Take(3).ToListAsync();

            return objects;
        }
    }
}