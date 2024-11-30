from app import db

class Deposito(db.Model):
    __tablename__ = 'depositos'

    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    name = db.Column(db.String(255), nullable=False)

    # Relación con DepositoProveedor
    proveedores = db.relationship('DepositoProveedor', backref='deposito', lazy=True)

    def __repr__(self):
        return f"<Deposito {self.name}>"