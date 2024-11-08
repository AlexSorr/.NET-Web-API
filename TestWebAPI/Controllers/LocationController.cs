
using API.Controllers.Base;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.Models;

[Route("api/[controller]")]
[ApiController]
public class LocationController : BulkUploadController<Location> {

    public LocationController(ApplicationDbContext context, ILogger<LocationController> logger, DataLoader dataLoader) : base(context, logger, dataLoader) { }
    
}