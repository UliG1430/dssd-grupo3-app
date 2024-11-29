from flask import Blueprint, jsonify, request
from flask_jwt_extended import jwt_required, get_jwt_identity
from app.models.order import Orden
from app.models.material import Material
from app.models.deposito import Deposito
from app.models.deposito_proveedor import DepositoProveedor 
from app import db

protected_bp = Blueprint('protected_bp', __name__)

@protected_bp.route('/protected', methods=['GET'])
@jwt_required()
def protected():
    current_user = get_jwt_identity()
    return jsonify(logged_in_as=current_user), 200

@protected_bp.route('/orders/<int:material_id>', methods=['GET'])
@jwt_required()  # Protect this route with JWT
def get_orders_by_material(material_id):
    # Get the user ID from the JWT token
    current_user_id = get_jwt_identity()

    # Query the database to find orders with the specified material_id and reservada == False
    orders = Orden.query.filter_by(material=material_id, reservada=False).all()

    # Serialize the result
    orders_list = [
        {
            "id": order.id,
            "punto_recoleccion": order.punto_recoleccion,
            "peso": order.peso,
            "material": order.material,
            "id_user": order.id_user,
            "reservada": order.reservada,
            "retirada": order.retirada
        } for order in orders
    ]

    return jsonify(orders_list)

@protected_bp.route('/orders/reserve/<int:order_id>', methods=['PATCH'])
@jwt_required()  # Protect this route with JWT
def reserve_order(order_id):
    # Query the order by its id
    order = Orden.query.get(order_id)
    
    # If the order doesn't exist, return a 404 error
    if order is None:
        return jsonify({"msg": "Order does not exist"}), 404
    
    # Check if the order is already reserved
    if order.reservada:
        return jsonify({"msg": "Order is already reserved"}), 409  # Conflict status code
    
    # If the order is not reserved, update its 'reservada' status
    order.reservada = True
    
    # Commit the change to the database
    try:
        db.session.commit()
        return jsonify({"msg": "Order has been successfully reserved"}), 200
    except Exception as e:
        db.session.rollback()
        return jsonify({"msg": "Error reserving the order", "error": str(e)}), 500
    
@protected_bp.route('/orders/retire/<int:order_id>', methods=['PATCH'])
@jwt_required()  # Protect this route with JWT
def retire_order(order_id):  # Renamed this function to avoid conflict
    # Query the order by its id
    order = Orden.query.get(order_id)
    
    # If the order doesn't exist, return a 404 error
    if order is None:
        return jsonify({"msg": "Order does not exist"}), 404
    
    # Check if the order is already retired
    if order.retirada:
        return jsonify({"msg": "Order has already been retired"}), 409  # Conflict status code
    
    # If the order is not retired, update its 'retirada' status
    order.retirada = True
    
    # Commit the change to the database
    try:
        db.session.commit()
        return jsonify({"msg": "Order has been successfully retired"}), 200
    except Exception as e:
        db.session.rollback()
        return jsonify({"msg": "Error retiring the order", "error": str(e)}), 500
    
@protected_bp.route('/add_deposito_proveedor', methods=['POST'])
@jwt_required()  # Protected with JWT
def add_deposito_proveedor():
    # Get the deposito_id and material_id from the request body
    deposito_id = request.json.get('deposito_id', None)
    material_id = request.json.get('material_id', None)

    # Validate that deposito_id and material_id are provided
    if deposito_id is None or material_id is None:
        return jsonify({"msg": "Both deposito_id and material_id must be provided"}), 400

    # Check if the deposito exists
    deposito = Deposito.query.get(deposito_id)
    if deposito is None:
        return jsonify({"msg": f"No deposit found with id {deposito_id}"}), 404

    # Check if the material exists
    material = Material.query.get(material_id)
    if material is None:
        return jsonify({"msg": f"No material found with id {material_id}"}), 404

    # Check if the deposito_proveedor already exists
    existing_entry = DepositoProveedor.query.filter_by(deposito_id=deposito_id, material_id=material_id).first()
    if existing_entry:
        return jsonify({"msg": "The deposito is already registered as a provider for this material"}), 409

    # If not, create a new deposito_proveedor entry
    new_deposito_proveedor = DepositoProveedor(deposito_id=deposito_id, material_id=material_id)
    
    # Add the new entry to the database
    try:
        db.session.add(new_deposito_proveedor)
        db.session.commit()
        return jsonify({"msg": "The deposito has been successfully registered as a provider for the material"}), 201
    except Exception as e:
        db.session.rollback()
        return jsonify({"msg": "Error adding deposito_proveedor", "error": str(e)}), 500
    
@protected_bp.route('/necesidades', methods=['GET'])
def get_needs():
    needs = [
        {"material": "Material A", "quantity": 10, "deposit": "Deposit 1"},
        {"material": "Material B", "quantity": 20, "deposit": "Deposit 2"},
        {"material": "Material C", "quantity": 30, "deposit": "Deposit 3"}
    ]
    return jsonify(needs), 200