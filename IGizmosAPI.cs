namespace ChristmasStory;

using System;
using UnityEngine;

public interface IGizmosAPI
{
    public Material GetDefaultMaterial();
    public void SetDefaultMaterialPass(int pass = 0);
    public void DrawWithReference(Transform reference, Action drawMethod);
    public void  DrawOnGlobalReference(Action drawMethod);
    public void DrawWithOrthoProjection(Action drawMethod);
    public void DrawAxis(float headSize, Color color, Vector3 offset);
    public void DrawTransform(Transform transform, float headSize, Color color);
    public void DrawCollider(Collider collider, Color color);
    public void DrawShape(Shape shape, Color color);
    public void DrawShapeBoundingSphere(Shape shape, Color color);
    public void DrawSimpleWireframeCube(Vector3 offset, Color color);
    public void DrawWireframeCube(Vector3 foward, Vector3 up, Vector3 right, Vector3 offset, Color color);
    public void DrawWireframeCircle(float radius, Vector3 normal, Vector3 up, Vector3 offset, Color color, int resolution = 3, float startAngle = 0f, float endAngle = 2f * Mathf.PI, bool isWholeCircle = true);
    public void DrawSimpleWireframeSphere(float radius, Vector3 offset, Color color, int resolution);
    public void DrawWireframeSphere(float radius, Vector3 offset, Vector3 foward, Vector3 up, Color color, int resolution = 3);
    public void DrawWireframeHemisphere(float radius, Vector3 offset, Vector3 foward, Vector3 up, Color color, int resolution = 3);
    public void DrawWireframeCapsule(float radius, Vector3 startPoint, Vector3 endPoint, Color color, int resolution = 3);
    public void DrawSimpleWireframeCapsule(float radius, float height, Vector3 up, Vector3 offset, Color color, int resolution = 3);
    public void DrawWireframeCone(float coneRadiusStart, float coneRadiusEnd, Vector3 coneStart, Vector3 coneEnd, Color color, int resolution = 3);
    public void DrawVector(Vector3 vector, float headSize, Vector3 offset, Color color);
}