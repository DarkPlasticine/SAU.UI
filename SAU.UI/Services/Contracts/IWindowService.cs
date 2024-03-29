﻿using System;

namespace SAU.UI.Services.Contracts
{
    public interface IWindowService
    {
        public void Show(Type windowType);

        public T Show<T>() where T : class;
    }
}