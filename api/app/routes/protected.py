from flask import Blueprint, jsonify
from flask_jwt_extended import jwt_required, get_jwt_identity

protected_bp = Blueprint('protected_bp', __name__)

@protected_bp.route('/protected', methods=['GET'])
@jwt_required()
def protected():
    current_user = get_jwt_identity()
    return jsonify(logged_in_as=current_user), 200
