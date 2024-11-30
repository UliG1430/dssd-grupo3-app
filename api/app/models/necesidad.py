from app import db

class Necesidad(db.Model):
    __tablename__ = 'necesidades'

    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    material = db.Column(db.String(50), nullable=False)  # Nombre del material
    cod_material = db.Column(db.String(10), nullable=False)  # Código del material
    quantity = db.Column(db.Integer, nullable=False)  # Cantidad necesaria
    deposito_id = db.Column(db.Integer, db.ForeignKey('depositos.id'), nullable=False)  # Relación con depósitos
    material_id = db.Column(db.Integer, db.ForeignKey('materiales.id'), nullable=False)  # Relación con materiales
    estado = db.Column(db.String(20), nullable=False, default="PENDIENTE")  # Estado de la necesidad

    # Relaciones
    deposito = db.relationship('Deposito', backref='necesidades')
    material_rel = db.relationship('Material', backref='necesidades')

    def __repr__(self):
        return f'<Necesidad {self.material} - {self.quantity}kg>'