#pragma warning disable 1591
using System;
using System.Runtime.Serialization;

namespace Rhino.Geometry
{
  [Serializable]
  public class Light : GeometryBase, ISerializable
  {
#if RDK_CHECKED
    /// <summary>
    /// Constructs a light that represents the Sun.
    /// </summary>
    /// <param name="northAngleDegrees">The angle of North in degrees.</param>
    /// <param name="azimuthDegrees">The Azimut angle value in degrees.</param>
    /// <param name="altitudeDegrees">The Altitude angle in degrees.</param>
    /// <returns>A new sun light.</returns>
    public static Light CreateSunLight(double northAngleDegrees, double azimuthDegrees, double altitudeDegrees)
    {
      Rhino.Runtime.HostUtils.CheckForRdk(true, true);
      IntPtr pSun = UnsafeNativeMethods.Rdk_SunNew();
      IntPtr pSunInterface = UnsafeNativeMethods.Rdk_SunInterface(pSun);
      UnsafeNativeMethods.Rdk_Sun_SetNorth(pSunInterface, northAngleDegrees);
      UnsafeNativeMethods.Rdk_Sun_SetAzimuthAltitude(pSunInterface, azimuthDegrees, altitudeDegrees);

      Light rc = new Light();
      IntPtr pLight = rc.NonConstPointer();
      UnsafeNativeMethods.Rdk_Sun_Light(pSunInterface, pLight);
      UnsafeNativeMethods.Rdk_SunDelete(pSun);
      return rc;
    }

    /// <summary>
    /// Constructs a light which simulates the Sun based on a given time and location on Earth.
    /// </summary>
    /// <param name="northAngleDegrees">The angle of North in degrees.</param>
    /// <param name="when">The time.</param>
    /// <param name="whenKind"></param>
    /// <param name="latitudeDegrees"></param>
    /// <param name="longitudeDegrees"></param>
    /// <returns></returns>
    /// <exception cref="Rhino.Runtime.RdkNotLoadedException"></exception>
    /// <exception cref="System.ArgumentException">if whenKind is set to Unspecified</exception>
    [Obsolete("Removed in favor of version that does not require a DateTimeKind since this is embedded in a DateTime. Will be removed in a future beta.")]
    public static Light CreateSunLight(double northAngleDegrees, DateTime when, DateTimeKind whenKind, double latitudeDegrees, double longitudeDegrees)
    {
      Rhino.Runtime.HostUtils.CheckForRdk(true, true);
      if (whenKind == DateTimeKind.Unspecified)
        throw new ArgumentException("whenKind must be specified");

      IntPtr pSun = UnsafeNativeMethods.Rdk_SunNew();
      IntPtr pSunInterface = UnsafeNativeMethods.Rdk_SunInterface(pSun);
      UnsafeNativeMethods.Rdk_Sun_SetNorth(pSunInterface, northAngleDegrees);
      UnsafeNativeMethods.Rdk_Sun_SetLatitudeLongitude(pSunInterface, latitudeDegrees, longitudeDegrees);

      bool local = whenKind== DateTimeKind.Local;
      UnsafeNativeMethods.Rdk_Sun_SetDateTime(pSunInterface, local, when.Year, when.Month, when.Day, when.Hour, when.Minute, when.Second);
      Light rc = new Light();
      IntPtr pLight = rc.NonConstPointer();
      UnsafeNativeMethods.Rdk_Sun_Light(pSunInterface, pLight);
      UnsafeNativeMethods.Rdk_SunDelete(pSun);
      return rc;
    }

    /// <summary>
    /// Create a light which simulates the sun based on a given time and location on earth
    /// Initializes a light which simulates the Sun.
    /// </summary>
    /// <param name="northAngleDegrees"></param>
    /// <param name="when"></param>
    /// <param name="latitudeDegrees"></param>
    /// <param name="longitudeDegrees"></param>
    /// <returns></returns>
    /// <exception cref="Rhino.Runtime.RdkNotLoadedException"></exception>
    /// <exception cref="System.ArgumentException">if whenKind is set to Unspecified</exception>
    public static Light CreateSunLight(double northAngleDegrees, DateTime when, double latitudeDegrees, double longitudeDegrees)
    {
      Rhino.Runtime.HostUtils.CheckForRdk(true, true);

      IntPtr pSun = UnsafeNativeMethods.Rdk_SunNew();
      IntPtr pSunInterface = UnsafeNativeMethods.Rdk_SunInterface(pSun);
      UnsafeNativeMethods.Rdk_Sun_SetNorth(pSunInterface, northAngleDegrees);
      UnsafeNativeMethods.Rdk_Sun_SetLatitudeLongitude(pSunInterface, latitudeDegrees, longitudeDegrees);

      bool local = (when.Kind == DateTimeKind.Local || when.Kind == DateTimeKind.Unspecified);
      UnsafeNativeMethods.Rdk_Sun_SetDateTime(pSunInterface, local, when.Year, when.Month, when.Day, when.Hour, when.Minute, when.Second);
      Light rc = new Light();
      IntPtr pLight = rc.NonConstPointer();
      UnsafeNativeMethods.Rdk_Sun_Light(pSunInterface, pLight);
      UnsafeNativeMethods.Rdk_SunDelete(pSun);
      return rc;
    }

    /// <summary>
    /// Create a light which simlulates the sun
    /// </summary>
    /// </summary>
    /// <param name="sun">A Sun object from the Rhino.Render namespace.</param>
    /// <returns>A light.</returns>
    public static Light CreateSunLight(Rhino.Render.Sun sun)
    {
      Rhino.Runtime.HostUtils.CheckForRdk(true, true);

      Rhino.Geometry.Light rc = new Rhino.Geometry.Light();
      IntPtr pLight = rc.NonConstPointer();
      IntPtr pConstSun = sun.ConstPointer();
      UnsafeNativeMethods.Rdk_Sun_Light(pConstSun, pLight);

      return rc;
    }
#endif

    internal Light(IntPtr native_ptr, object parent)
      : base(native_ptr, parent, -1)
    { }

    internal override GeometryBase DuplicateShallowHelper()
    {
      return new Light(IntPtr.Zero, null);
    }

    /// <summary>
    /// Initializes a new light.
    /// </summary>
    public Light()
    {
      IntPtr pLight = UnsafeNativeMethods.ON_Light_New();
      ConstructNonConstObject(pLight);
    }

    /// <summary>
    /// Protected constructor used in serialization.
    /// </summary>
    /// <param name="info">Serialization data.</param>
    /// <param name="context">Serialization stream.</param>
    protected Light(SerializationInfo info, StreamingContext context)
      : base (info, context)
    {
    }

    /// <summary>
    /// Gets or sets a value that defines if the light is turned on (true) or off (false).
    /// </summary>
    public bool IsEnabled
    {
      get
      {
        IntPtr pConstThis = ConstPointer();
        return UnsafeNativeMethods.ON_Light_IsEnabled(pConstThis);
      }
      set
      {
        IntPtr pThis = NonConstPointer();
        UnsafeNativeMethods.ON_Light_SetEnabled(pThis, value);
      }
    }

    const int idxLightStyle = 0;
    const int idxCoordinateSystem = 1;
    int GetInt(int which)
    {
      IntPtr pConstThis = ConstPointer();
      return UnsafeNativeMethods.ON_Light_GetInt(pConstThis, which);
    }
    void SetInt(int which, int val)
    {
      IntPtr pThis = NonConstPointer();
      UnsafeNativeMethods.ON_Light_SetInt(pThis, which, val);
    }

    /// <summary>
    /// Gets or sets a light style on this camera.
    /// </summary>
    public LightStyle LightStyle
    {
      get { return (LightStyle)GetInt(idxLightStyle); }
      set { SetInt(idxLightStyle, (int)value); }
    }

    /// <summary>
    /// Gets a value indicating whether the light style
    /// is <see cref="LightStyle"/> CameraPoint or WorldPoint.
    /// </summary>
    public bool IsPointLight
    {
      get
      {
        LightStyle ls = LightStyle;
        return ls == LightStyle.CameraPoint || ls == LightStyle.WorldPoint;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the light style
    /// is <see cref="LightStyle"/> CameraDirectional or WorldDirectional.
    /// </summary>
    public bool IsDirectionalLight
    {
      get
      {
        LightStyle ls = LightStyle;
        return ls == LightStyle.CameraDirectional || ls == LightStyle.WorldDirectional;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the light style
    /// is <see cref="LightStyle"/> CameraSpot or WorldSpot.
    /// </summary>
    public bool IsSpotLight
    {
      get
      {
        LightStyle ls = LightStyle;
        return ls == LightStyle.CameraSpot || ls == LightStyle.WorldSpot;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the light style
    /// is <see cref="LightStyle"/> WorldLinear.
    /// </summary>
    public bool IsLinearLight
    {
      get { return LightStyle == LightStyle.WorldLinear; }
    }

    /// <summary>
    /// Gets a value indicating whether the light style
    /// is <see cref="LightStyle"/> WorldRectangular.
    /// </summary>
    public bool IsRectangularLight
    {
      get { return LightStyle == LightStyle.WorldRectangular; }
    }

    /// <summary>
    /// Gets a value indicating whether this object is a Sun light.
    /// </summary>
    public bool IsSunLight
    {
      get 
      {
        Rhino.Runtime.HostUtils.CheckForRdk(true, true);
        IntPtr pConstThis = ConstPointer();
        return UnsafeNativeMethods.Rdk_Sun_IsSunLight(pConstThis); 
      }
    }

    /// <summary>
    /// Gets a value, determined by LightStyle, that explains whether
    /// the camera diretions are relative to World or Camera spaces.
    /// </summary>
    public DocObjects.CoordinateSystem CoordinateSystem
    {
      get { return (DocObjects.CoordinateSystem)GetInt(idxCoordinateSystem);}
    }

    /// <summary>
    /// Gets or sets the light or 3D position or location.
    /// </summary>
    public Point3d Location
    {
      get
      {
        IntPtr pConstThis = ConstPointer();
        Point3d rc = new Point3d();
        UnsafeNativeMethods.ON_Light_GetLocation(pConstThis, ref rc);
        return rc;
      }
      set
      {
        IntPtr pThis = NonConstPointer();
        UnsafeNativeMethods.ON_Light_SetLocation(pThis, value);
      }
    }

    const int idxDirection = 0;
    const int idxPerpendicularDirection = 1;
    const int idxLength = 2;
    const int idxWidth = 3;
    Vector3d GetVector(int which)
    {
      IntPtr pConstThis = ConstPointer();
      Vector3d rc = new Vector3d();
      UnsafeNativeMethods.ON_Light_GetVector(pConstThis, ref rc, which);
      return rc;
    }
    void SetVector(int which, Vector3d v)
    {
      IntPtr pThis = NonConstPointer();
      UnsafeNativeMethods.ON_Light_SetVector(pThis, v, which);
    }

    /// <summary>
    /// Gets or sets the vector direction of the camera.
    /// </summary>
    public Vector3d Direction
    {
      get{ return GetVector(idxDirection); }
      set { SetVector(idxDirection, value); }
    }

    /// <summary>
    /// Gets a perpendicular vector to the camera direction.
    /// </summary>
    public Vector3d PerpendicularDirection
    {
      get { return GetVector(idxPerpendicularDirection); }
    }

    const int idxIntensity = 0;
    const int idxPowerWatts = 1;
    const int idxPowerLumens = 2;
    const int idxPowerCandela = 3;
    const int idxSpotAngleRadians = 4;
    const int idxSpotExponent = 5;
    const int idxHotSpot = 6;
    const int idxShadowIntensity = 7;
    double GetDouble(int which)
    {
      IntPtr pConstThis = ConstPointer();
      return UnsafeNativeMethods.ON_Light_GetDouble(pConstThis, which);
    }
    void SetDouble(int which, double val)
    {
      IntPtr pThis = NonConstPointer();
      UnsafeNativeMethods.ON_Light_SetDouble(pThis, which, val);
    }

    public double Intensity
    {
      get { return GetDouble(idxIntensity); }
      set { SetDouble(idxIntensity, value); }
    }

    public double PowerWatts
    {
      get { return GetDouble(idxPowerWatts); }
      set { SetDouble(idxPowerWatts, value); }
    }
    public double PowerLumens
    {
      get { return GetDouble(idxPowerLumens); }
      set { SetDouble(idxPowerLumens, value); }
    }
    public double PowerCandela
    {
      get { return GetDouble(idxPowerCandela); }
      set { SetDouble(idxPowerCandela, value); }
    }

    const int idxAmbient = 0;
    const int idxDiffuse = 1;
    const int idxSpecular = 2;
    System.Drawing.Color GetColor(int which)
    {
      IntPtr pConstThis = ConstPointer();
      int abgr = UnsafeNativeMethods.ON_Light_GetColor(pConstThis, which);
      return System.Drawing.ColorTranslator.FromWin32(abgr);
    }
    void SetColor(int which, System.Drawing.Color c)
    {
      IntPtr pThis = NonConstPointer();
      int argb = c.ToArgb();
      UnsafeNativeMethods.ON_Light_SetColor(pThis, which, argb);
    }

    public System.Drawing.Color Ambient
    {
      get { return GetColor(idxAmbient); }
      set { SetColor(idxAmbient, value); }
    }
    public System.Drawing.Color Diffuse
    {
      get { return GetColor(idxDiffuse); }
      set { SetColor(idxDiffuse, value); }
    }
    public System.Drawing.Color Specular
    {
      get { return GetColor(idxSpecular); }
      set { SetColor(idxSpecular, value); }
    }

    /// <summary>
    /// attenuation settings (ignored for "directional" and "ambient" lights)
    /// attenuation = 1/(a0 + d*a1 + d^2*a2) where d = distance to light
    /// </summary>
    /// <param name="a0"></param>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    public void SetAttenuation(double a0, double a1, double a2)
    {
      IntPtr pThis = NonConstPointer();
      UnsafeNativeMethods.ON_Light_SetAttenuation(pThis, a0, a1, a2);
    }
    /// <summary>
    /// attenuation settings (ignored for "directional" and "ambient" lights)
    /// attenuation = 1/(a0 + d*a1 + d^2*a2) where d = distance to light
    /// </summary>
    /// <param name="d"></param>
    /// <returns>0 if a0 + d*a1 + d^2*a2 &lt;= 0</returns>
    public double GetAttenuation(double d)
    {
      IntPtr pConstThis = ConstPointer();
      return UnsafeNativeMethods.ON_Light_GetAttenuation(pConstThis, d);
    }

    /// <summary>
    /// ignored for non-spot lights
    /// angle = 0 to pi/2  (0 to 90 degrees)
    /// </summary>
    public double SpotAngleRadians
    {
      get{ return GetDouble(idxSpotAngleRadians); }
      set{ SetDouble(idxSpotAngleRadians, value); }
    }

    /// <summary>
    /// The spot exponent varies from 0.0 to 128.0 and provides
    /// an exponential interface for controling the focus or 
    /// concentration of a spotlight (like the 
    /// OpenGL GL_SPOT_EXPONENT parameter).  The spot exponent
    /// and hot spot parameters are linked; changing one will
    /// change the other.
    /// A hot spot setting of 0.0 corresponds to a spot exponent of 128.
    /// A hot spot setting of 1.0 corresponds to a spot exponent of 0.0.
    /// </summary>
    public double SpotExponent
    {
      get { return GetDouble(idxSpotExponent); }
      set { SetDouble(idxSpotExponent, value); }
    }

    /// <summary>
    /// The hot spot setting runs from 0.0 to 1.0 and is used to
    /// provides a linear interface for controling the focus or 
    /// concentration of a spotlight.
    /// A hot spot setting of 0.0 corresponds to a spot exponent of 128.
    /// A hot spot setting of 1.0 corresponds to a spot exponent of 0.0.
    /// </summary>
    public double HotSpot
    {
      get { return GetDouble(idxHotSpot); }
      set { SetDouble(idxHotSpot, value); }
    }

    public bool GetSpotLightRadii(out double innerRadius, out double outerRadius)
    {
      innerRadius = 0;
      outerRadius = 0;
      IntPtr pConstThis = ConstPointer();
      return UnsafeNativeMethods.ON_Light_GetSpotLightRadii(pConstThis, ref innerRadius, ref outerRadius);
    }

    /// <summary>
    /// linear and rectangular light parameter
    /// (ignored for non-linear/rectangular lights)
    /// </summary>
    public Vector3d Length
    {
      get { return GetVector(idxLength); }
      set { SetVector(idxLength, value); }
    }

    /// <summary>
    /// linear and rectangular light parameter
    /// (ignored for non-linear/rectangular lights)
    /// </summary>
    public Vector3d Width
    {
      get { return GetVector(idxWidth); }
      set { SetVector(idxWidth, value); }
    }

    public double SpotLightShadowIntensity
    {
      get { return GetDouble(idxShadowIntensity); }
      set { SetDouble(idxShadowIntensity, value); }
    }

    public string Name
    {
      get
      {
        IntPtr pConstThis = ConstPointer();
        using (Rhino.Runtime.StringHolder sh = new Rhino.Runtime.StringHolder())
        {
          IntPtr pString = sh.NonConstPointer();
          UnsafeNativeMethods.ON_Light_GetName(pConstThis, pString);
          return sh.ToString();
        }
      }
      set
      {
        IntPtr pThis = NonConstPointer();
        UnsafeNativeMethods.ON_Light_SetName(pThis, value);
      }
    }
  }
}
