// src/services/bonitaService.ts

// Simulamos la llamada a la API con un retraso
export const iniciarSesionBonita = async () => {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        // Puedes ajustar este comportamiento para simular errores o respuestas correctas
        const success = true; // Cambia a false para simular un error
  
        if (success) {
          resolve({ message: 'Autenticación exitosa' });
        } else {
          reject(new Error('Error en la autenticación de Bonita'));
        }
      }, 2000); // Simulamos un delay de 2 segundos
    });
  };
  