using System.Net;
using System.Net.Mime;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public interface IImageRepository
{
    //Define the method signature for the Upload method
    Task<Image> Upload(Image image);
}