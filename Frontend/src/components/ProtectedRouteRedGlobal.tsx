// src/components/ProtectedRouteRedGlobal.tsx
import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

// Función para verificar la validez del token JWT
const isValidToken = (token: string | null) => {
  if (!token) return false;

  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => `%${('00' + c.charCodeAt(0).toString(16)).slice(-2)}`)
        .join('')
    );

    const payload = JSON.parse(jsonPayload);
    const exp = payload.exp;
    if (exp && Date.now() >= exp * 1000) {
      return false; // Token expirado
    }
    return true; // Token válido
  } catch (error) {
    return false; // Token inválido
  }
};

const ProtectedRouteRedGlobal: React.FC = () => {
  const token = localStorage.getItem('redGlobalToken'); // Obtenemos el token JWT

  // Verificamos si el token es válido
  return isValidToken(token) ? <Outlet /> : <Navigate to="/red-global-recicladores" />; // Redirigir al login si el token no es válido
};

export default ProtectedRouteRedGlobal;
