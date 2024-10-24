from flask import Blueprint, jsonify, request
from flask_jwt_extended import create_access_token
from werkzeug.security import check_password_hash  # Importa esta función
from app.models.user import User

auth_bp = Blueprint('auth_bp', __name__)

@auth_bp.route('/login', methods=['POST'])
def login():
    username = request.json.get('username', None)
    password = request.json.get('password', None)

    # Busca al usuario por su nombre de usuario
    user = User.query.filter_by(username=username).first()
    
    # Verifica si el usuario existe y si la contraseña es correcta
    if not user or not check_password_hash(user.password, password):
        return jsonify({"msg": "Bad username or password"}), 401

    # Si la autenticación es exitosa, crea un JWT token
    access_token = create_access_token(identity=user.id)
    return jsonify(access_token=access_token), 200
