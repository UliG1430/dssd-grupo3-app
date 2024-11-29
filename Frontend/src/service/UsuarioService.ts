interface Usuario {
    id: number;
    UsuarioNombre: string;
    Password: string;
    comenzoRecorrido: boolean;
    caseId: number;
    rol: string;
    seleccionoPaquete: boolean;
    paqueteId: number;
  }

  export const getUsuarioByUsername = async (username: string): Promise<Usuario> => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Usuario/${username}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });


      if (response.status !== 200) {
        throw new Error(`Error al obtener el usuario: ${response.status}`);
      }

      const data: Usuario = await response.json();
      return data; // Retornamos la respuesta si fue exitosa
    } catch (error) {
      console.error('Error en la llamada a getUsuarioByUsername:', error);
      throw error;
    }
  };

  export const updateUsuarioById = async (id: number,CaseId: number, ComenzoRecorrido: boolean, Rol: string, SeleccionoPaquete: boolean, PaqueteId: number): Promise<void> => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Usuario/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ CaseId , ComenzoRecorrido, Rol, PaqueteId, SeleccionoPaquete })
      });


      if (response.status !== 200) {
        throw new Error(`Error al obtener el usuario: ${response.status}`);
      }

      console.log(`Se actualizo el usuario: ${id}`);
    } catch (error) {
      console.error('Error en la llamada a updateUsuarioById:', error);
      throw error;
    }
  };