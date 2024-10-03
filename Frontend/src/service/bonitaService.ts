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
    console.log("Token de Bonita recibido:", data);
    return data;  // Esto contiene el token
  } catch (error) {
    console.error('Error en la llamada de login:', error);
    throw error;
  }
};

export const getProcessId = async (processName: string, token: string) => {
  try {
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/process/${processName}`, {
      method: 'GET',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error('Error al obtener el processId');
    }

    const data = await response.json();
    console.log("Process ID recibido:", data);
    return data;  // Retorna el `processId`
  } catch (error) {
    console.error('Error en la llamada a getProcessId:', error);
    throw error;
  }
};

export const startProcessById = async (processId: string, token: string) => {
  try {
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/startprocess/${processId}`, {
      method: 'GET',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error('Error al iniciar el proceso');
    }

    const data = await response.json();
    console.log('Proceso iniciado:', data);
    return data; // Retorna la instancia del proceso
  } catch (error) {
    console.error('Error en la llamada a startProcessById:', error);
    throw error;
  }
};

export const logoutBonita = async () => {
  try {
    await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/Logout`, {  // Llama al endpoint de logout en tu backend
      method: 'POST',
    });
    console.log('Sesión de Bonita cerrada.');
  } catch (error) {
    console.error('Error cerrando la sesión de Bonita:', error);
  }
};
export const completeTask = async (caseId: string, token: string) => {
  try {
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/completeActivity/${caseId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-Bonita-API-Token': token,
      },
    });

    if (!response.ok) {
      throw new Error('Error al completar la actividad');
    }

    const data = await response.json();
    return data;  // Devolver la respuesta del servidor
  } catch (error) {
    console.error('Error al completar la actividad:', error);
    throw error;
  }
};
