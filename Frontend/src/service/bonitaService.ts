// src/services/bonitaService.ts

const getBonitaToken = () => {
  return localStorage.getItem('bonitaToken');
};

export const loginBonita = async (username: string, password: string) => {
  try {
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/Login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ username, password })
    });

    if (response.status !== 200) {
      throw new Error('Error al obtener la siguiente tarea');
    }

    const data = await response.json();
    console.log("Token de Bonita recibido:", data);
    localStorage.setItem('bonitaToken', data.token); // Store the token in localStorage
    return data;
  } catch (error) {
    console.error('Error en la llamada de login:', error);
    throw error;
  }
};

export const logoutBonita = async () => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/Logout`, {
      method: 'GET',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });
    console.log('Sesión de Bonita cerrada.');
  } catch (error) {
    console.error('Error cerrando la sesión de Bonita:', error);
  }
}

export const getProcessId = async (processName: string) => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/process/${processName}`, {
      method: 'GET',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });

    if (response.status !== 200) {
      throw new Error('Error al obtener la siguiente tarea');
    }

    const data = await response.json();
    console.log("Process ID recibido:", data);
    return data.processId;  // Retorna el `processId`
  } catch (error) {
    console.error('Error en la llamada a getProcessId:', error);
    throw error;
  }
};

export const startProcessById = async (processId: number) => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/startprocess/${processId}`, {
      method: 'GET',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });

    if (response.status !== 200) {
      throw new Error('Error al obtener la siguiente tarea');
    }

    const data = await response.json();
    console.log('Proceso iniciado:', data);
    return data.processInstance; // Retorna la instancia del proceso
  } catch (error) {
    console.error('Error en la llamada a startProcessById:', error);
    throw error;
  }
};

export const getNextTaskId = async (caseId: string): Promise<any> => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    console.log(`${import.meta.env.VITE_API_BASE_URL}/Bonita/getNextTask/${caseId}`);
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/getNextTask/${caseId}`, {
      method: 'GET',
      headers: {
      'X-Bonita-API-Token': token,
      'Content-Type': 'application/json',
      },
    });

    if (response.status !== 200) {
      throw new Error('Error al obtener la siguiente tarea');
    }

    const data = await response.json();
    console.log('Siguiente tarea obtenida:', data);
    return data.nextTaskId; // Retorna el id de la tarea
  } catch (error) {
    console.error('Error en la llamada a getNextTask:', error);
    throw error;
  }
};

export const assignTask = async (taskId: string, userId: string): Promise<void> => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/task/${taskId}/assign/${userId}`, {
      method: 'PUT',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });

    if (response.status !== 200) {
      throw new Error('Error al asignar la tarea');
    }

    console.log(`Tarea ${taskId} asignada al usuario ${userId}`);
  } catch (error) {
    console.error('Error en la llamada a assignTask:', error);
    throw error;
  }
};

export const executeTask = async (taskId: string): Promise<void> => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/task/${taskId}/execute`, {
      method: 'POST',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });

    if (response.status !== 200) {
      throw new Error('Error al obtener la siguiente tarea');
    }

    console.log(`Tarea ${taskId} ejecutada`);
  } catch (error) {
    console.error('Error en la llamada a executeTask:', error);
    throw error;
  }
};

export const setCaseVariable = async (caseId: string, variableName: string, value: boolean): Promise<void> => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/case/${caseId}/variable/${variableName}`, {
      method: 'PUT',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        type: 'java.lang.Boolean',
        value: value,
      }),
    });

    if (response.status !== 200) {
      throw new Error('Error al obtener la siguiente tarea');
    }

    console.log(`Variable ${variableName} del caso ${caseId} establecida a ${value}`);
  } catch (error) {
    console.error('Error en la llamada a setCaseVariable:', error);
    throw error;
  }
};

export const getUsuarioIdByUsername = async (username: string): Promise<number> => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/${username}/id`, {
      method: 'GET',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });

    if (response.status !== 200) {
      throw new Error('Error al obtener la siguiente tarea');
    }

    console.log(`Se obtuvo el id del usuario: ${username}: ${response}`);
    const data = await response.json(); // Retorna el id del usuario
    return data.userId;
  } catch (error) {
    console.error('Error en la llamada a setCaseVariable:', error);
    throw error;
  }
};

export const getTaskById = async (taskId: string): Promise<any> => {
  try {
    const token: string = getBonitaToken() != null ? getBonitaToken()! : '';
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bonita/task/${taskId}`, {
      method: 'GET',
      headers: {
        'X-Bonita-API-Token': token,
        'Content-Type': 'application/json',
      },
    });

    if (response.status !== 200) {
      throw new Error('Error al obtener la siguiente tarea');
    }

    const data = await response.json();
    console.log('Tarea obtenida:', data);
    return data;
  } catch (error) {
    console.error('Error en la llamada a getTaskById:', error);
    throw error;
  }
};

