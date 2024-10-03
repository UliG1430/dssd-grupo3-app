// src/services/bonitaService.ts

export const loginBonita = async () => {
    try {
      // Credenciales fijas para el login
      const username = "walter.bates";
      const password = "bpm";
  
      const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/Login`, {  // Cambia la URL según tu backend
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password })  // Enviamos las credenciales fijas
      });
  
      if (!response.ok) {
        throw new Error('Error en el login de Bonita');
      }
  
      const data = await response.json();
      console.log("entre")
      console.log(data)
      return data;  // Esto contiene el token u otros datos
    } catch (error) {
      console.error('Error en la llamada de login:', error);
      throw error;
    }
  };
  