namespace E_Club.DTOs.Home;

public record HomeResponse(
    List<BannerResponse> Banners,
    List<ServiceResponse> QuickServices,
    EventResponse? FeaturedEvent,
    int UnreadNotificationsCount
);
