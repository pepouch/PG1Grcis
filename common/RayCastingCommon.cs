﻿using System;
using System.Collections.Generic;
using System.Drawing;
using MathSupport;
using OpenTK;

// Common code for ray-based rendering.
namespace Rendering
{
  #region Interfaces

  /// <summary>
  /// Function from continuous 2D space (virtual screen) to some color space.
  /// Usually does all the rendering job except for anti-aliasing.
  /// </summary>
  public interface IImageFunction
  {
    /// <summary>
    /// Domain width.
    /// </summary>
    double Width
    {
      get;
      set;
    }

    /// <summary>
    /// Domain height.
    /// </summary>
    double Height
    {
      get;
      set;
    }

    /// <summary>
    /// Computes one image sample. Simple variant, w/o an integration support.
    /// </summary>
    /// <param name="x">Horizontal coordinate.</param>
    /// <param name="y">Vertical coordinate.</param>
    /// <param name="color">Computed sample color.</param>
    /// <returns>Hash-value used for adaptive subsampling.</returns>
    long GetSample ( double x, double y, double[] color );

    /// <summary>
    /// Computes one image sample. Internal integration support.
    /// </summary>
    /// <param name="x">Horizontal coordinate.</param>
    /// <param name="y">Vertical coordinate.</param>
    /// <param name="rank">Rank of this sample, 0 <= rank < total (for integration).</param>
    /// <param name="total">Total number of samples (for integration).</param>
    /// <param name="rnd">Global (per-thread) instance of the random generator.</param>
    /// <param name="color">Computed sample color.</param>
    /// <returns>Hash-value used for adaptive subsampling.</returns>
    long GetSample ( double x, double y, int rank, int total, RandomJames rnd, double[] color );
  }

  /// <summary>
  /// Delegate function for thread-selection.
  /// </summary>
  /// <param name="unit">Number of working unit (thread selected for unit==0 is leader).</param>
  public delegate bool ThreadSelector ( long unit );

  /// <summary>
  /// Algorithm capable of synthesizing raster image from virtual 3D scene.
  /// Usually associated with an IImageFunction object (which does the actual job).
  /// </summary>
  public interface IRenderer
  {
    /// <summary>
    /// Image width in pixels.
    /// </summary>
    int Width
    {
      get;
      set;
    }

    /// <summary>
    /// Image height in pixels.
    /// </summary>
    int Height
    {
      get;
      set;
    }

    /// <summary>
    /// Gamma pre-compensation (gamma encoding, compression). Values 0.0 or 1.0 mean "no compensation".
    /// </summary>
    double Gamma
    {
      get;
      set;
    }

    /// <summary>
    /// Cell-size for adaptive rendering, 0 or 1 to turn off adaptivitiy.
    /// </summary>
    int Adaptive
    {
      get;
      set;
    }

    /// <summary>
    /// Current progress object (can be null).
    /// </summary>
    Progress ProgressData
    {
      get;
      set;
    }

    /// <summary>
    /// Renders the single pixel of an image.
    /// </summary>
    /// <param name="x">Horizontal coordinate.</param>
    /// <param name="y">Vertical coordinate.</param>
    /// <param name="color">Computed pixel color.</param>
    /// <param name="rnd">Shared random generator.</param>
    void RenderPixel ( int x, int y, double[] color, RandomJames rnd );

    /// <summary>
    /// Renders the given rectangle into the given raster image.
    /// </summary>
    /// <param name="image">Pre-initialized raster image.</param>
    /// <param name="rnd">Shared random generator.</param>
    void RenderRectangle ( Bitmap image, int x1, int y1, int x2, int y2, RandomJames rnd );

    /// <summary>
    /// Renders the given rectangle into the given raster image.
    /// Has to be re-entrant since this code is started in multiple parallel threads.
    /// </summary>
    /// <param name="image">Pre-initialized raster image.</param>
    /// <param name="sel">Selector for this working thread.</param>
    /// <param name="rnd">Thread-specific random generator.</param>
    void RenderRectangle ( Bitmap image, int x1, int y1, int x2, int y2, ThreadSelector sel, RandomJames rnd );
  }

  /// <summary>
  /// Ray generator (camera for ray-based methods).
  /// </summary>
  public interface ICamera
  {
    /// <summary>
    /// Width / Height of the viewing area (viewport, frustum).
    /// </summary>
    double AspectRatio
    {
      get;
      set;
    }

    /// <summary>
    /// Viewport width.
    /// </summary>
    double Width
    {
      get;
      set;
    }

    /// <summary>
    /// Viewport height.
    /// </summary>
    double Height
    {
      get;
      set;
    }

    /// <summary>
    /// Ray-generator. Simple variant, w/o an integration support.
    /// </summary>
    /// <param name="x">Origin position within a viewport (horizontal coordinate).</param>
    /// <param name="y">Origin position within a viewport (vertical coordinate).</param>
    /// <param name="p0">Ray origin.</param>
    /// <param name="p1">Ray direction vector.</param>
    /// <returns>True if the ray (viewport position) is valid.</returns>
    bool GetRay ( double x, double y, out Vector3d p0, out Vector3d p1 );

    /// <summary>
    /// Ray-generator. Internal integration support.
    /// </summary>
    /// <param name="x">Origin position within a viewport (horizontal coordinate).</param>
    /// <param name="y">Origin position within a viewport (vertical coordinate).</param>
    /// <param name="rank">Rank of this ray, 0 <= rank < total (for integration).</param>
    /// <param name="total">Total number of rays (for integration).</param>
    /// <param name="rnd">Global (per-thread) instance of the random generator.</param>
    /// <param name="p0">Ray origin.</param>
    /// <param name="p1">Ray direction vector.</param>
    /// <returns>True if the ray (viewport position) is valid.</returns>
    bool GetRay ( double x, double y, int rank, int total, RandomJames rnd, out Vector3d p0, out Vector3d p1 );
  }

  /// <summary>
  /// General light source.
  /// </summary>
  public interface ILightSource
  {
    /// <summary>
    /// Returns intensity (incl. color) of the source contribution to the given scene point.
    /// Simple variant, w/o an integration support.
    /// </summary>
    /// <param name="intersection">Scene point (only world coordinates and normal vector are needed).</param>
    /// <param name="dir">Direction to the source is set here (zero vector for omnidirectional source). Not normalized!</param>
    /// <returns>Intensity vector in current color space or null if the point is not lit.</returns>
    double[] GetIntensity ( Intersection intersection, out Vector3d dir );

    /// <summary>
    /// Returns intensity (incl. color) of the source contribution to the given scene point.
    /// Internal integration support.
    /// </summary>
    /// <param name="intersection">Scene point (only world coordinates and normal vector are needed).</param>
    /// <param name="rank">Rank of this sample, 0 <= rank < total (for integration).</param>
    /// <param name="total">Total number of samples (for integration).</param>
    /// <param name="rnd">Global (per-thread) instance of the random generator.</param>
    /// <param name="dir">Direction to the source is set here (zero vector for omnidirectional source). Not normalized!</param>
    /// <returns>Intensity vector in current color space or null if the point is not lit.</returns>
    double[] GetIntensity ( Intersection intersection, int rank, int total, RandomJames rnd, out Vector3d dir );
  }

  public enum ReflectionComponent
  {
    DIFFUSE = 1,
    SPECULAR_REFLECTION = 2,
    SPECULAR_REFRACTION = 4,
    SPECULAR = 6,
    ALL = 7
  }

  /// <summary>
  /// Reflection model - deals with local interaction between light and material surface.
  /// Entry point of the light ray is assumed to be equal to the exit point.
  /// </summary>
  public interface IReflectanceModel
  {
    IMaterial DefaultMaterial ();

    double[] ColorReflection ( Intersection intersection, Vector3d input, Vector3d output, ReflectionComponent comp );

    double[] ColorReflection ( IMaterial material, Vector3d normal, Vector3d input, Vector3d output, ReflectionComponent comp );
  }

  /// <summary>
  /// Abstract material description.
  /// Each IReflectionModel should define its own derivation with specific properties.
  /// </summary>
  public interface IMaterial : ICloneable
  {
    /// <summary>
    /// Base surface color.
    /// </summary>
    double[] Color
    {
      get;
      set;
    }

    /// <summary>
    /// Coefficient of transparency.
    /// </summary>
    double Kt
    {
      get;
      set;
    }

    /// <summary>
    /// Absolute index of refraction.
    /// </summary>
    double n
    {
      get;
      set;
    }
  }

  /// <summary>
  /// Any object capable of ray-intersection in 3D.
  /// </summary>
  public interface IIntersectable
  {
    /// <summary>
    /// Computes the complete intersection of the given ray with the object. 
    /// </summary>
    /// <param name="p0">Ray origin.</param>
    /// <param name="p1">Ray direction vector.</param>
    /// <returns>Sorted list of intersection records.</returns>
    LinkedList<Intersection> Intersect ( Vector3d p0, Vector3d p1 );

    /// <summary>
    /// Complete all relevant items in the given Intersection object.
    /// </summary>
    /// <param name="inter">Intersection instance to complete.</param>
    void CompleteIntersection ( Intersection inter );
  }

  /// <summary>
  /// Texture object: general value-modulator (value = color, normal vector..).
  /// </summary>
  public interface ITexture
  {
    /// <summary>
    /// Apply the relevant value-modulation in the given Intersection instance.
    /// Simple variant, w/o an integration support.
    /// </summary>
    /// <param name="inter">Data object to modify.</param>
    /// <returns>Hash value (texture signature) for adaptive subsampling.</returns>
    long Apply ( Intersection inter );

    /// <summary>
    /// Apply the relevant value-modulation in the given Intersection instance.
    /// Internal integration support.
    /// </summary>
    /// <param name="inter">Data object to modify.</param>
    /// <param name="rank">Rank of this sample, 0 <= rank < total (for integration).</param>
    /// <param name="total">Total number of samples (for integration).</param>
    /// <param name="rnd">Global (per-thread) instance of the random generator.</param>
    /// <returns>Hash value (texture signature) for adaptive subsampling.</returns>
    long Apply ( Intersection inter, int rank, int total, RandomJames rnd );
  }

  /// <summary>
  /// Data container for Ray-based scene rendering: complete scene definition including camera.
  /// </summary>
  public interface IRayScene
  {
    /// <summary>
    /// Scene model (whatever is able to compute ray intersections).
    /// </summary>
    IIntersectable Intersectable
    {
      get;
      set;
    }

    /// <summary>
    /// Background color.
    /// </summary>
    double[] BackgroundColor
    {
      get;
      set;
    }

    /// <summary>
    /// Camera = primary ray generator.
    /// </summary>
    ICamera Camera
    {
      get;
      set;
    }

    /// <summary>
    /// Set of light sources.
    /// </summary>
    ICollection<ILightSource> Sources
    {
      get;
      set;
    }
  }

  /// <summary>
  /// Abstract time-dependency of an object.
  /// Each instance has to be able to clone itself (multi-thread implementations).
  /// </summary>
  public interface ITimeDependent : ICloneable
  {
    /// <summary>
    /// Starting (minimal) time in seconds.
    /// </summary>
    double Start
    {
      get;
      set;
    }

    /// <summary>
    /// Ending (maximal) time in seconds.
    /// </summary>
    double End
    {
      get;
      set;
    }

    /// <summary>
    /// Current time in seconds.
    /// </summary>
    double Time
    {
      get;
      set;
    }
  }

  #endregion

  #region Basic objects

  /// <summary>
  /// Data class keeping info about current progress of a computation.
  /// </summary>
  public class Progress
  {
    /// <summary>
    /// Relative amount of work finished so far (0.0f to 1.0f).
    /// </summary>
    public float Finished
    {
      get;
      set;
    }

    /// <summary>
    /// Optional message. Any string.
    /// </summary>
    public string Message
    {
      get;
      set;
    }

    /// <summary>
    /// Continue in an associated computation.
    /// </summary>
    public bool Continue
    {
      get;
      set;
    }

    /// <summary>
    /// Sync interval in milliseconds.
    /// </summary>
    public long SyncInterval
    {
      get;
      set;
    }

    /// <summary>
    /// Any message from computing unit to the GUI main.
    /// </summary>
    public virtual void Sync ( Object msg )
    {
    }

    /// <summary>
    /// Set all the harmless values.
    /// </summary>
    public Progress ()
    {
      Finished = 0.0f;
      Message = "";
      Continue = true;
      SyncInterval = 8000L;
    }
  }

  /// <summary>
  /// Intersection of a ray with a solid surface.
  /// </summary>
  public class Intersection : IComparable
  {
    /// <summary>
    /// True if the ray enters a solid interior of an object here (transition from an air to solid material).
    /// (mandatory: Solid, InnerNode)
    /// </summary>
    public bool Enter;

    /// <summary>
    /// True if the ray enters solid geometry from outside (ray and surface normal have different directions).
    /// (mandatory: Solid)
    /// </summary>
    public bool Front;

    /// <summary>
    /// Parametric coordinate on the ray
    /// (mandatory: Solid).
    /// </summary>
    public double T;

    /// <summary>
    /// World coordinates of an intersection.
    /// (deferred: Scene graph)
    /// </summary>
    public Vector3d CoordWorld;

    /// <summary>
    /// Object coordinates of an intersection (object is an animation unit).
    /// (deferred: Scene graph)
    /// </summary>
    public Vector3d CoordObject;

    /// <summary>
    /// Local (solid's) coordinates of an intersection.
    /// (deferred: Solid)
    /// </summary>
    public Vector3d CoordLocal;

    /// <summary>
    /// 2D texture coordinates.
    /// (deferred: Solid)
    /// </summary>
    public Vector2d TextureCoord;

    /// <summary>
    /// Normal vector in world coordinates.
    /// (deferred: Solid)
    /// </summary>
    public Vector3d Normal;

    /// <summary>
    /// Transform matrix from the local (Solid) space to the World space.
    /// (deferred: Scene graph)
    /// </summary>
    public Matrix4d LocalToWorld;

    /// <summary>
    /// Transform matrix from the World space to the local (Solid) space.
    /// (deferred: Scene graph)
    /// </summary>
    public Matrix4d WorldToLocal;

    /// <summary>
    /// Transform matrix from the local (Solid) space to the Object space.
    /// (deferred: Scene graph)
    /// </summary>
    public Matrix4d LocalToObject;

    /// <summary>
    /// Current surface color, used by textures and utilized by rendering algorithm in the end.
    /// (deferred: Scene graph, textures..)
    /// </summary>
    public double[] SurfaceColor;

    /// <summary>
    /// Current reflectance model.
    /// (deferred: Scene graph, textures..)
    /// </summary>
    public IReflectanceModel ReflectanceModel;

    /// <summary>
    /// Matching material record.
    /// (deferred: Scene graph, textures..)
    /// </summary>
    public IMaterial Material;

    /// <summary>
    /// Priority-list of relevant textures (less important to more important).
    /// </summary>
    public LinkedList<ITexture> Textures;

    /// <summary>
    /// Solid this intersection comes from..
    /// (mandatory: Solid)
    /// </summary>
    public ISolid Solid;

    /// <summary>
    /// Supplement data object for intersection completion.
    /// (mandatory: Solid)
    /// </summary>
    public object SolidData;

    /// <summary>
    /// Number of rays casted into the scene.
    /// </summary>
    public static long countRays = 0L;

    /// <summary>
    /// Number of individual ray x solid intersections computed
    /// (each surface has its intersection).
    /// </summary>
    public static long countIntersections = 0L;

    public Intersection ( ISolid s )
    {
      Solid = s;
    }

    /// <summary>
    /// Complete non-mandatory (deferred) values in the Intersection instance.
    /// </summary>
    public void Complete ()
    {
      if ( Solid != null )
      {
        // world coordinates:
        LocalToWorld = Solid.ToWorld();
        WorldToLocal = LocalToWorld;
        WorldToLocal.Invert();
        Vector3d.TransformPosition( ref CoordLocal, ref LocalToWorld, out CoordWorld );

        // object coordinates:
        LocalToObject = Solid.ToObject();
        Vector3d.TransformPosition( ref CoordLocal, ref LocalToObject, out CoordObject );

        // appearance:
        ReflectanceModel = (IReflectanceModel)Solid.GetAttribute( PropertyName.REFLECTANCE_MODEL );
        if ( ReflectanceModel == null )
          ReflectanceModel = new PhongModel();
        Material = (IMaterial)Solid.GetAttribute( PropertyName.MATERIAL );
        if ( Material == null )
          Material = ReflectanceModel.DefaultMaterial();
        double[] col = (double[])Solid.GetAttribute( PropertyName.COLOR );
        SurfaceColor = (double[])((col != null) ? col.Clone() : Material.Color.Clone());
        Textures = Solid.GetTextures();

        // Solid is responsible for completing remaining values:
        Solid.CompleteIntersection( this );
        Normal.Normalize();
        if ( Enter != Front )
          Vector3d.Multiply( ref Normal, -1.0, out Normal );
      }

      if ( SurfaceColor == null )
        SurfaceColor = new double[] { 0.0, 0.2, 0.3 };
    }

    public static Intersection FirstIntersection ( LinkedList<Intersection> list, ref Vector3d p1 )
    {
      if ( list == null || list.Count < 1 )
        return null;

      double p1Squared = p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z;
      foreach ( Intersection i in list )
        if ( i.T > 0.0 && i.T * i.T * p1Squared > 1.0e-8 )
          return i;

      return null;
    }

    /// <summary>
    /// Compares the given t against the stored intersection.
    /// Uses local-space epsilon tolerance and provided direction vector.
    /// </summary>
    /// <returns>True if this intersection is far than t.</returns>
    public bool Far ( double t, ref Vector3d p1 )
    {
      if ( T <= t ) return false;

      double p1Squared = p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z;
      t = T - t;
      return( t * t * p1Squared > 1.0e-8 );
    }

    /// <summary>
    /// Canonic sort order: by a T value.
    /// </summary>
    /// <param name="obj">Instance to be compared to..</param>
    /// <returns>-1, 0 or 1.</returns>
    public int CompareTo ( object obj )
    {
      Intersection i = (Intersection)obj;
      return T.CompareTo( i.T );
    }
  }

  #endregion
}
