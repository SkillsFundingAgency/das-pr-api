﻿using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.Authorization;

[ExcludeFromCodeCoverage]
public class NoneRequirement : IAuthorizationRequirement;