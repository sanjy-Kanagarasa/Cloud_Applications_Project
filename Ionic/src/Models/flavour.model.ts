import { Location } from './driver';

export class Flavour{
  constructor(public name: string, public amount: number, public price: number){}
}
export class FlavourUpdate{
  constructor(public name: string, public price: number){}
}
export class FlavourUpdateJson{
  constructor(public Flavours: FlavourUpdate[]){}
}
export class Icecream{
  constructor(public iceCream: Flavour[]){}
}
export class ShoppingCart{
  constructor(public cart: Icecream[], public location: Location){}
}
