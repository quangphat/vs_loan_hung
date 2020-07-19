using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class ProfileRepository : RepositoryBase, IProfileRepository
    {
        public ProfileRepository(IConfiguration configuration) : base(configuration)
        {
        }
        //public async Gets()
        //{

        //}
    }
}
