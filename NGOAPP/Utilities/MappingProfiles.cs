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
        CreateMap<Event, EventView>();
        CreateMap<EventTicket, EventTicketView>();
        CreateMap<TicketType, BaseViewModelI>();
        CreateMap<Contact, ContactView>();
        CreateMap<Session, SessionView>();
        CreateMap<Speaker, SpeakerView>();
        CreateMap<Location, LocationView>();
        CreateMap<LocationType, BaseViewModelI>();
        CreateMap<Status, BaseViewModelI>();
        CreateMap<EventType, BaseViewModel>();
        CreateMap<EventCategory, BaseViewModel>();
        CreateMap<EventSubCategory, BaseViewModelI>();
        CreateMap<Schedule, ScheduleView>();
        CreateMap<Group, GroupView>();

        CreateMap<GroupUser, UserView>()
            .ConstructUsing((src, dest) => dest.Mapper.Map<UserView>(src.User));

        // always map the status name from an object of type Status to a string
        CreateMap<Status, string>().ConvertUsing(s => s.Name);
    }

}
