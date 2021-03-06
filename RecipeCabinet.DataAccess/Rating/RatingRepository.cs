using Microsoft.Extensions.Configuration;
using RecipeCabinet.DataAccess.Common;
using RecipeCabinet.Domain.Rating;
using System.Data.SqlClient;
using System;

namespace RecipeCabinet.DataAccess.Rating
{
    public class RatingRepository : BaseRepository, IRatingRepository
    {
        // SPROCS
        private const string RATING_CREATE_SPROC = "Rating_Create";

        private const string RATING_GETBYID_SPROC = "Rating_GetById";
        private const string RATING_UPDATE_SPROC = "Rating_Update";
        private const string RATING_DELETE_SPROC = "Rating_Delete";

        public RatingRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public RatingModel Create(RatingModel rating)
        {
            SqlParameter messageParam = new SqlParameter("Name", rating.Message);
            SqlParameter ratingParam = new SqlParameter("Type", rating.Rating);
            SqlParameter recipeParam = new SqlParameter("Recipe", rating.Recipe);
            SqlDataReader reader = ExecuteReader(RATING_CREATE_SPROC, System.Data.CommandType.StoredProcedure, messageParam, ratingParam, recipeParam);
            while (reader.Read())
            {
                // This might not be right.
                rating.Id = int.Parse(reader["Id"].ToString());
            }
            return rating;
        }

        public RatingModel GetById(int id)
        {
            RatingModel model = null;
            SqlParameter param = new SqlParameter("Id", id);
            SqlDataReader reader = ExecuteReader(RATING_GETBYID_SPROC, System.Data.CommandType.StoredProcedure, param);
            while (reader.Read())
            {
                model = new RatingModel
                {
                    Id = int.Parse(reader["Id"].ToString()),
                    CreatedOn = DateTime.Parse(reader["CreatedOn"].ToString()),
                    LastUpdatedOn = DateTime.Parse(reader["LastUpdatedOn"].ToString()),
                    Message = reader["Message"].ToString(),
                    Rating = int.Parse(reader["Rating"].ToString()),
                    Recipe = int.Parse(reader["Recipe"].ToString())
                };
            }
            return model;
        }

        public RatingModel Update(RatingModel rating)
        {
            SqlParameter param = new SqlParameter("Id", rating.Id);
            SqlParameter messageParam = new SqlParameter("Message", rating.Message);
            SqlParameter ratingParam = new SqlParameter("Rating", rating.Rating);
            SqlParameter recipeParam = new SqlParameter("Recipe", rating.Recipe);
                ExecuteNonReader(RATING_UPDATE_SPROC, System.Data.CommandType.StoredProcedure, param, messageParam, ratingParam, recipeParam);

            return rating;
        }

        public void Delete(int id)
        {
            SqlParameter param = new SqlParameter("Id", id);
            ExecuteNonReader(RATING_DELETE_SPROC, System.Data.CommandType.StoredProcedure, param);
        }
    }
}
}
