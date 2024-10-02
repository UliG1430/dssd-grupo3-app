// src/routes/AppRoutes.tsx
import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Home from '../pages/Home';
import CargarRecoleccion from '../pages/CargarRecoleccion'; // Página donde se carga el formulario después del click

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/cargar-recoleccion" element={<CargarRecoleccion />} /> {/* Ruta a la página del proceso */}
    </Routes>
  );
};

export default AppRoutes;
