using Spendy.Data;

namespace Spendy.Data.Entities;

public sealed class CategoryEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Icon { get; set; } = string.Empty;
	public CategoryScope Scope { get; set; }
}
