// src/App.tsx
import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import Navbar from './components/Navbar'; // Navbar
import AppRoutes from './routes/AppRoutes'; // Rutas centralizadas en su archivo

function App() {
  return (
    <Router>
      <Navbar />
      <AppRoutes />
    </Router>
  );
}

export default App;
