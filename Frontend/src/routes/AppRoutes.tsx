// src/routes/AppRoutes.tsx
import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Home from '../pages/Home';
import ProtectedRoute from '../components/ProtectedRoute';
import Login from '../pages/Login';
import CargarRecoleccion from '../pages/CargarRecoleccion';
import StartRecoleccion from '../pages/StartRecoleccion'; // Import the new page
import VisitarPunto from '../pages/VisitarPunto';
import EsperarCobro from '../pages/EsperarCobro';
import EntregarPaquete from '../pages/EntregarPaquete';
import Paquetes from '../pages/Paquetes';
import AnalizarOrdenes from '../pages/AnalizarOrdenes';

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/iniciar-sesion" element={<Login />} />
      <Route element={<ProtectedRoute />}>
        <Route path="/cargar-recoleccion" element={<CargarRecoleccion />} />
        <Route path="/comenzar-recoleccion" element={<StartRecoleccion />} /> {/* Add the new protected route */}
        <Route path="/visitar-punto" element={<VisitarPunto />} />
        <Route path="/esperar-cobro" element={<EsperarCobro />} />
        <Route path="/entregar-paquetes" element={<EntregarPaquete />} />
        <Route path="/paquetes" element={<Paquetes />} />
        <Route path="/analizar-ordenes/:paqueteId" element={<AnalizarOrdenes />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;