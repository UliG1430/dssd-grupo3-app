from app import db

class DepositoProveedor(db.Model):
    __tablename__ = 'deposito_proveedor'

    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    
    # Foreign Keys for Deposito and Material
    deposito_id = db.Column(db.Integer, db.ForeignKey('depositos.id'), nullable=False)
    material_id = db.Column(db.Integer, db.ForeignKey('materiales.id'), nullable=False)

    def __repr__(self):
        return f"<DepositoProveedor {self.deposito_id} - {self.material_id}>"
