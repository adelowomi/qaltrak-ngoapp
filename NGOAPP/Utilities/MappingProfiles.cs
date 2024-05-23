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
    }

}
