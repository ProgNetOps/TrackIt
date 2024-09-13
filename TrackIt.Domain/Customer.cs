
namespace TrackIt.Domain;
/// <summary>
/// The Identity class for customer users of the application, representing a discriminator in the database
/// </summary>
public class Customer:ApplicationUser
{
    public string CompanyName { get; set; }
    public string? OfficeAddress { get; set; }

}
