using Godot;
using System;

/// <summary>
/// Recurso que representa los datos de un nivel en el menú.
/// </summary>
[GlobalClass] // Esto permite crear el recurso desde el editor
public partial class NivelData : Resource
{
    [Export] public string Nombre;
    
    [Export(PropertyHint.File, "*.tscn")]
    public string RutaEscena;
}