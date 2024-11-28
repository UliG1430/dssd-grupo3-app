from flask import Flask, jsonify
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_jwt_extended import JWTManager
from flasgger import Swagger
from flask_cors import CORS  # Importamos flask-cors
import os

# Initializing extensions
db = SQLAlchemy()
migrate = Migrate()
jwt = JWTManager()

def create_app():
    app = Flask(__name__)

    # Configuración
    app.config['SQLALCHEMY_DATABASE_URI'] = os.getenv('DATABASE_URL', 'postgresql://postgres:postgres@db:5432/api_database')
    app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
    app.config['JWT_SECRET_KEY'] = os.getenv('JWT_SECRET_KEY', 'dssd-grupo3')
    app.config['JWT_ACCESS_TOKEN_EXPIRES'] = 3600  # Token expires in 1 hour

    # Habilitar CORS para permitir solicitudes desde tu frontend
    CORS(app, origins="http://localhost:5173")  # Cambia por la URL de tu frontend

    # Inicializamos las extensiones
    db.init_app(app)
    migrate.init_app(app, db)
    jwt.init_app(app)

    # Inicializar Flasgger (Swagger UI)
    Swagger(app)

    @jwt.expired_token_loader
    def expired_token_callback(jwt_header, jwt_payload):
        return jsonify({"msg": "The token has expired"}), 401

    @jwt.invalid_token_loader
    def invalid_token_callback(error):
        return jsonify({"msg": "Invalid token"}), 401

    # Registrar los Blueprints
    from app.auth.routes import auth_bp
    from app.routes.protected import protected_bp

    app.register_blueprint(auth_bp, url_prefix='/auth')
    app.register_blueprint(protected_bp, url_prefix='/api')

    return app
