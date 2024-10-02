// src/services/bonitaService.ts

export const loginBonita = async (): Promise<string | null> => {
    try {
      const response = await fetch('http://localhost:8080/bonita/loginservice', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: new URLSearchParams({
          username: 'admin', // Cambia según tu configuración
          password: 'admin', // Cambia según tu configuración
          redirect: 'false'
        })
      });
  
      if (response.ok) {
        const token = response.headers.get('X-Bonita-API-Token');
        return token;
      } else {
        console.error('Error en la autenticación con Bonita');
        return null;
      }
    } catch (error) {
      console.error('Error en la conexión con Bonita', error);
      return null;
    }
  };
  
  export const startProcessInstance = async (token: string, processId: string, planData: any): Promise<void> => {
    try {
      const response = await fetch(`http://localhost:8080/bonita/API/bpm/process/${processId}/instantiation`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'X-Bonita-API-Token': token
        },
        body: JSON.stringify(planData)
      });
  
      if (response.ok) {
        console.log('Proceso iniciado exitosamente');
      } else {
        console.error('Error al iniciar el proceso en Bonita');
      }
    } catch (error) {
      console.error('Error en la comunicación con Bonita', error);
    }
  };
  