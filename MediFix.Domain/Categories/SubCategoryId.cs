﻿namespace MediFix.Domain.Categories;

public record SubCategoryId(Guid Value) : StronglyTypedId<Guid>(Value);