using MediFix.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Seed;

internal static class CategoriesSeed
{
    public static ModelBuilder SeedCategories(this ModelBuilder modelBuilder)
    {
        var categories = GetCategories().ToList();
        var subCategories = GetSubCategories(categories);

        modelBuilder.Entity<Category>()
            .HasData(categories);

        modelBuilder.Entity<SubCategory>()
            .HasData(subCategories);

        return modelBuilder;
    }


    private static IEnumerable<Category> GetCategories()
    {
        var plumbing = new Category(
            CategoryId.Create(),
            "Plumbing");

        var airConditioning = new Category(
            CategoryId.Create(),
            "Air conditioning");

        return [plumbing, airConditioning];
    }

    private static IEnumerable<SubCategory> GetSubCategories(IReadOnlyList<Category> categories)
    {
        var plumbing = categories[0];

        var toilet = new SubCategory(
            SubCategoryId.Create(),
            "Toilet",
            plumbing.Id);

        var tap = new SubCategory(
            SubCategoryId.Create(),
            "Tap",
            plumbing.Id);

        var waterBar = new SubCategory(
            SubCategoryId.Create(),
            "Water Bar",
            plumbing.Id);

        var airConditioning = categories[1];

        var cool = new SubCategory(
            SubCategoryId.Create(),
            "Air conditioner does not cool",
            airConditioning.Id);

        var noise = new SubCategory(
            SubCategoryId.Create(),
            "Noisy air conditioner",
            airConditioning.Id);

        return [toilet, tap, waterBar, cool, noise];
    }
}
