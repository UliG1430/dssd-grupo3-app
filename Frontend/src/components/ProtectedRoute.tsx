// src/components/ProtectedRoute.tsx
import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

const ProtectedRoute: React.FC = () => {
  const token = localStorage.getItem('bonitaToken');

  return token ? <Outlet /> : <Navigate to="/" />;
};

export default ProtectedRoute;