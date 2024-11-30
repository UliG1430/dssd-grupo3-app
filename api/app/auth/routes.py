from flask import Blueprint, jsonify, request
from flask_jwt_extended import create_access_token
from app.models.user import User

auth_bp = Blueprint('auth_bp', __name__)

@auth_bp.route('/login', methods=['POST'])
def login():
    """
    Login to the application.
    ---
    tags:
      - Authentication
    consumes:
      - application/json
    parameters:
      - name: username
        in: body
        type: string
        required: true
        description: The user's username.
      - name: password
        in: body
        type: string
        required: true
        description: The user's password.
    responses:
      200:
        description: Successfully logged in and JWT token returned.
        schema:
          type: object
          properties:
            access_token:
              type: string
              description: The JWT access token.
              example: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
            rol:
              type: string
              description: The role of the user.
              example: "A"
      401:
        description: Unauthorized - Bad username or password.
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Bad username or password.
    """

    username = request.json.get('username', None)
    password = request.json.get('password', None)

    # Busca al usuario por su nombre de usuario
    user = User.query.filter_by(username=username).first()
    
    # Verifica si el usuario existe y si la contraseña es correcta (texto plano)
    if not user or user.password != password:
        return jsonify({"msg": "Bad username or password"}), 401

    # Si la autenticación es exitosa, crea un JWT token
    access_token = create_access_token(identity=str(user.id))  # Convertir user.id a string

    # Devuelve el token y el rol del usuario
    return jsonify(access_token=access_token, rol=user.rol), 200