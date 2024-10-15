from flask import Blueprint, jsonify, request
from flask_jwt_extended import create_access_token
from app.models.user import User

auth_bp = Blueprint('auth_bp', __name__)

@auth_bp.route('/login', methods=['POST'])
def login():
    username = request.json.get('username', None)
    password = request.json.get('password', None)

    # Aquí debes validar el usuario desde tu base de datos
    user = User.query.filter_by(username=username).first()
    if not user or not (user.password == password):
        return jsonify({"msg": "Bad username or password"}), 401

    # Create JWT token
    access_token = create_access_token(identity=user.id)
    return jsonify(access_token=access_token), 200

