// Login a Bonita (ya está)
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
    console.log("Token de Bonita recibido:", data);
    return data;  // Esto contiene el token u otros datos
  } catch (error) {
    console.error('Error en la llamada de login:', error);
    throw error;
  }
};

// Obtener el Process ID de Bonita
export const getProcessId = async (processName: string) => {
  try {
    // Recuperamos el token almacenado en localStorage
    const token = localStorage.getItem('bonitaToken');

    if (!token) {
      throw new Error('No se encontró el token en localStorage.');
    }

    // Realizamos la solicitud GET para obtener el processId
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/process/${processName}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'X-Bonita-API-Token': token  // Agregamos el token a la cabecera
      },
    });

    if (!response.ok) {
      throw new Error('Error al obtener el process ID de Bonita.');
    }

    const data = await response.json();
    console.log("Process ID recibido:", data.processId);
    return data.processId;  // Devolvemos el processId
  } catch (error) {
    console.error('Error al obtener el Process ID:', error);
    throw error;
  }
};
