from flask import Blueprint, jsonify, request
from flask_jwt_extended import jwt_required, get_jwt_identity
from app.models.order import Orden
from app.models.material import Material
from app.models.deposito import Deposito
from app.models.deposito_proveedor import DepositoProveedor 
from app import db
from flasgger import Swagger

protected_bp = Blueprint('protected_bp', __name__)

@protected_bp.route('/protected', methods=['GET'])
@jwt_required()
def protected():
    """
    Verify if the user is authenticated.
    ---
    tags:
      - Authentication
    security:
      - Bearer: []
    responses:
      200:
        description: The user is authenticated and a JWT is valid.
        schema:
          type: object
          properties:
            logged_in_as:
              type: string
              description: The current user's ID.
              example: 1
      401:
        description: Unauthorized - Invalid or missing JWT token.
        schema:
          type: object
          properties:
            msg:
              type: string
              example: The token has expired or is invalid.
    """
    current_user = get_jwt_identity()
    return jsonify(logged_in_as=current_user), 200

@protected_bp.route('/orders/<int:material_id>', methods=['GET'])
@jwt_required()  # Protect this route with JWT
def get_orders_by_material(material_id):
    """
    Retrieve orders by material ID.
    ---
    tags:
      - Orders
    parameters:
      - name: material_id
        in: path
        type: integer
        required: true
        description: ID of the material to filter orders
    responses:
      200:
        description: A list of orders
        schema:
          type: array
          items:
            type: object
            properties:
              id:
                type: integer
              punto_recoleccion:
                type: string
              peso:
                type: integer
              material:
                type: integer
              id_user:
                type: integer
              reservada:
                type: boolean
              retirada:
                type: boolean
      401:
        description: Unauthorized - Invalid or missing JWT token
    """
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
    """
    Mark a specific order as reserved.
    ---
    tags:
      - Orders
    parameters:
      - name: order_id
        in: path
        type: integer
        required: true
        description: ID of the order to reserve
    responses:
      200:
        description: Successfully reserved the order
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Order has been successfully reserved
      404:
        description: Order not found
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Order does not exist
      409:
        description: Order is already reserved
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Order is already reserved
      500:
        description: Error reserving the order
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Error reserving the order
            error:
              type: string
    """

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
    """
    Mark a specific order as retired.
    ---
    tags:
      - Orders
    parameters:
      - name: order_id
        in: path
        type: integer
        required: true
        description: ID of the order to retire
    responses:
      200:
        description: Successfully retired the order
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Order has been successfully retired
      404:
        description: Order not found
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Order does not exist
      409:
        description: Order is already retired
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Order has already been retired
      500:
        description: Error retiring the order
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Error retiring the order
            error:
              type: string
    """

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
    """
    Register a deposit as a provider for a specific material.
    ---
    tags:
      - DepositoProveedor
    parameters:
      - name: deposito_id
        in: body
        type: integer
        required: true
        description: ID of the deposit
      - name: material_id
        in: body
        type: integer
        required: true
        description: ID of the material
    responses:
      201:
        description: The deposit was successfully registered as a provider for the material
        schema:
          type: object
          properties:
            msg:
              type: string
              example: The deposito has been successfully registered as a provider for the material
      400:
        description: Missing deposito_id or material_id
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Both deposito_id and material_id must be provided
      404:
        description: Deposit or material not found
        schema:
          type: object
          properties:
            msg:
              type: string
              example: No deposit/material found with the provided ID
      409:
        description: The deposit is already registered as a provider for the material
        schema:
          type: object
          properties:
            msg:
              type: string
              example: The deposito is already registered as a provider for this material
      500:
        description: Error adding deposito_proveedor
        schema:
          type: object
          properties:
            msg:
              type: string
              example: Error adding deposito_proveedor
            error:
              type: string
    """

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