﻿namespace FoodApp.Api.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
