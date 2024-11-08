﻿using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.SubCategories.GetSubCategories;

public record GetSubCategoriesRequest(Guid? CategoryId, bool WithInactive) 
    : IQuery<SubCategoriesResponse>;