using AppDAL.DBModels;
using AppModel;

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBAL.BusinessConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Appuser, LoginUser>();            
        }
    }
}
