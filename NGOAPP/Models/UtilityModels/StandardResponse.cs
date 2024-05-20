using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace NGOAPP;

public class StandardResponse<T> : Link
{
    public bool Status { get; set; }

    public string? Message { get; set; }

    public T Data { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public object? Errors { get; set; }

    [JsonIgnore]
    public Link Self { get; set; }

    public StandardResponse()
    {
    }

    private StandardResponse(bool status)
    {
        Status = status;
    }

    private StandardResponse(bool status, string message)
    {
        Status = status;
        Message = message;
    }

    private StandardResponse(bool status, string message, T data)
    {
        Status = status;
        Message = message;
        Data = data;
    }

    private StandardResponse(bool status, string message, T data, HttpStatusCode statusCode)
    {
        Status = status;
        Message = message;
        Data = data;
        StatusCode = statusCode;
    }

    public static StandardResponse<T> Create()
    {
        return new StandardResponse<T>();
    }

    public static StandardResponse<T> Create(bool status)
    {
        return new StandardResponse<T>(status);
    }

    public static StandardResponse<T> Create(bool status, string message)
    {
        return new StandardResponse<T>(status, message);
    }

    public static StandardResponse<T> Create(bool status, string message, T data)
    {
        return new StandardResponse<T>(status, message, data);
    }

    public StandardResponse<T> AddStatusCode(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        return this;
    }

    public StandardResponse<T> AddStatusMessage(string message)
    {
        Message = message;
        return this;
    }

    public static StandardResponse<T> Error(string message)
    {
        return new StandardResponse<T>(status: false, message, default(T), HttpStatusCode.InternalServerError);
    }

    public static StandardResponse<T> Unauthorized()
    {
        return new StandardResponse<T>(status: false, "", default(T), HttpStatusCode.Unauthorized);
    }

    public static StandardResponse<T> Error(string message, HttpStatusCode statusCode)
    {
        return new StandardResponse<T>(status: false, message, default(T), statusCode);
    }

    public static StandardResponse<T> Ok(string message)
    {
        return new StandardResponse<T>(status: true, message, default(T), HttpStatusCode.OK);
    }

    public static StandardResponse<T> Ok(T data)
    {
        return new StandardResponse<T>(status: true, StandardResponseMessages.SUCCESSFUL, data, HttpStatusCode.OK);
    }

    public static StandardResponse<T> NotFound(string message)
    {
        return new StandardResponse<T>(status: false, message, default(T), HttpStatusCode.NotFound);
    }

    public static StandardResponse<T> Ok()
    {
        return new StandardResponse<T>(status: true, StandardResponseMessages.SUCCESSFUL, default(T), HttpStatusCode.OK);
    }

    public static StandardResponse<T> Failed()
    {
        return new StandardResponse<T>(status: false, StandardResponseMessages.UNSUCCESSFUL);
    }

    public static StandardResponse<T> Failed(string message)
    {
        return new StandardResponse<T>(status: false, message, default(T), HttpStatusCode.InternalServerError);
    }

    public static StandardResponse<T> Failed(string message, HttpStatusCode statusCode)
    {
        return new StandardResponse<T>(status: false, message, default(T), statusCode);
    }

    public StandardResponse<T> Build()
    {
        return this;
    }

    public StandardResponse<T> AddData(T data)
    {
        Data = data;
        return this;
    }
}


[ApiController]
[Route("api/[controller]")]
public class StandardControllerBase : ControllerBase
{
    protected ObjectResult Result<T>(StandardResponse<T> response)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return Ok(response);
            case HttpStatusCode.NotFound:
                return NotFound(response);
            case HttpStatusCode.BadRequest:
                return BadRequest(response);
            case HttpStatusCode.InternalServerError:
                return BadRequest(response);
            case HttpStatusCode.Unauthorized:
                return Unauthorized(response);
            default:
                return Ok(response);
        }
    }
}

