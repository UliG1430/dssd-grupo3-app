// src/routes/AppRoutes.tsx
import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Home from '../pages/Home';
import ProtectedRoute from '../components/ProtectedRoute';
import Login from '../pages/Login';
import CargarRecoleccion from '../pages/CargarRecoleccion';
import StartRecoleccion from '../pages/StartRecoleccion';
import VisitarPunto from '../pages/VisitarPunto';
import EsperarCobro from '../pages/EsperarCobro';
import EntregarPaquete from '../pages/EntregarPaquete';
import Paquetes from '../pages/Paquetes';
import AnalizarOrdenes from '../pages/AnalizarOrdenes';
import RegistrarResultado from '../pages/RegistrarResultado';
import ProtectedRouteRedGlobal from '../components/ProtectedRouteRedGlobal';
import HomeRedGlobal from '../pages/HomeRedGlobal';
import LoginRedGlobal from '../pages/LoginRedGlobal';
import Necesidades from '../pages/Necesidades';
import OrdenesDistribucion from '../pages/OrdenesDistribucion'; // Importamos la nueva página
import EsperarEvaluacion from '../pages/EsperarEvaluacion';
import Evaluaciones from '../pages/Evaluaciones';
import RealizarEvaluacion from '../pages/RealizarEvaluacion';

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/iniciar-sesion" element={<Login />} />

      <Route element={<ProtectedRoute />}>
        <Route path="/red-global-recicladores" element={<LoginRedGlobal />} />
        <Route path="/cargar-recoleccion" element={<CargarRecoleccion />} />
        <Route path="/comenzar-recoleccion" element={<StartRecoleccion />} />
        <Route path="/visitar-punto" element={<VisitarPunto />} />
        <Route path="/esperar-cobro" element={<EsperarCobro />} />
        <Route path="/entregar-paquetes" element={<EntregarPaquete />} />
        <Route path="/paquetes" element={<Paquetes />} />
        <Route path="/analizar-ordenes/:paqueteId" element={<AnalizarOrdenes />} />
        <Route path="/registrar-resultado/:caseId" element={<RegistrarResultado />} />
        <Route path="/esperar-evaluacion" element={<EsperarEvaluacion />} />
        <Route path="/evaluaciones" element={<Evaluaciones />} />
        <Route path="/realizar-evaluacion" element={<RealizarEvaluacion />} />
      </Route>
      <Route element={<ProtectedRouteRedGlobal />}>
        <Route path="/home-red-global" element={<HomeRedGlobal />} />
        <Route path="/necesidades" element={<Necesidades />} />
        <Route path="/ordenes-distribucion" element={<OrdenesDistribucion />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;