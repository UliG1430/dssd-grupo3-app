from app import db
from sqlalchemy import ForeignKey
from sqlalchemy.orm import relationship

class Orden(db.Model):
    __tablename__ = 'orden'

    # Columns
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    punto_recoleccion = db.Column(db.String(255), nullable=False)
    peso = db.Column(db.Integer, nullable=False)
    
    # Foreign Key linking to the user and material tables
    material = db.Column(db.Integer, ForeignKey('materiales.id'), nullable=False)
    id_user = db.Column(db.Integer, ForeignKey('users.id'), nullable=False)
    
    # Boolean fields, with default values
    reservada = db.Column(db.Boolean, default=False, nullable=False)
    retirada = db.Column(db.Boolean, default=False, nullable=False)

    # Relationship to the User model
    material_relation = relationship('Material', backref='ordenes')  # Relationship to Material

    def __repr__(self):
        return f"<Orden {self.id}: User {self.id_user} Material {self.id_material}>"
