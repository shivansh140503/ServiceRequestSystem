namespace ServiceRequest.Models.ServiceRequestModels;
public sealed record ServiceRequestModel
(
    int Id,
    string Title,
    string Description,
    string Status,
    string CreatedBy,
    DateTime CreatedDate
);
