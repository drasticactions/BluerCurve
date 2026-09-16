namespace BluerCurve.Icons;

public static class BluecurveIconContext
{
    public const string Actions = "actions";
    public const string Apps = "apps";
    public const string Devices = "devices";
    public const string Emblems = "emblems";
    public const string MimeTypes = "mimetypes";
    public const string Places = "places";
    public const string Status = "status";

    public static IReadOnlyList<string> All { get; } = [Actions, Apps, Devices, Emblems, MimeTypes, Places, Status];
}
