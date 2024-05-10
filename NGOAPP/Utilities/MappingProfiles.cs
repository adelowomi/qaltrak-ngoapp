using AutoMapper;
using NGOAPP.Models.IdentityModels;

namespace NGOAPP;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        // CreateMap<UserModel, User>();
        CreateMap<User, UserView>();

        // paystack mapping profiles
       
    }

}
