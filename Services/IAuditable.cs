namespace BarberApi.Services;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime LastModifiedAt { get; set; }
    Guid? CreatedBy { get; set; }
    Guid? UpdatedBy { get; set; }
}