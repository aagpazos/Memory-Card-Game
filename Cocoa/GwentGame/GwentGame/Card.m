//
//  Card.m
//  GwentGame
//
//  Created by Adrian on 20/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import "Card.h"

@implementation Card

#define XDIM 751
#define YDIM 751
#define DIMENSION 310

static NSRect funcRect = {-XDIM, -YDIM, 2*XDIM, 2*YDIM};

@synthesize cartaDelante, cartaAtras, rectangulo, imageName, abierta;

-(id) initWithCartaDelante:(NSImage *)cartaDelante withCartaAtras:(NSImage *)cartaAtras withRect:(NSRect)rect withImageName:(NSString *)imageName
{
    self = [super init];
    if(!self)
        return nil;
    rectPath = [[NSBezierPath alloc]init];
    [self setCartaDelante:cartaDelante];
    [self setCartaAtras:cartaAtras];
    [self setRectangulo:rect];
    [self setImageName:imageName];
    [self setAbierta:NO];
    [rectPath appendBezierPathWithRect:[self rectangulo]];
    tf = [NSAffineTransform transform];
    return self;
}


+(id)randomCard
{
    Card *newCard;
    NSRect aRect;
    static NSString *randomNameCard[] =
    {
        @"Ciri.jpg",
        @"Eredin.jpg",
        @"Jaskier.jpg",
        @"Triss.jpg",
        @"Geralt.jpg",
        @"Yennefer.jpg",
        @"Beauclair.jpg",
        @"KaerMorhen.png",
        @"Nilfgaard.jpg",
        @"Novigrad.jpg",
        @"Oxenfurt.jpg"
    };
    
    NSString *nameCard = randomNameCard[random() % 11];
    NSImage *cartaDel = [NSImage imageNamed:nameCard];
    NSImage *cartaBack = [NSImage imageNamed:@"back.jpg"];
    
    aRect.size.height = aRect.size.width = DIMENSION;
    aRect.origin.x = funcRect.origin.x + random() % (int)(funcRect.size.width - DIMENSION);
    aRect.origin.y = funcRect.origin.y + random() % (int)(funcRect.size.height - DIMENSION);
                                                        
    newCard = [[Card alloc]initWithCartaDelante:cartaDel withCartaAtras:cartaBack withRect:aRect withImageName:nameCard];
    return newCard;
}

+(id)copyCardWithOtherOrigin:(NSString *)nombreCarta
{
    Card *newCard;
    NSRect aRect;
    NSImage *cartaDelante = [NSImage imageNamed:nombreCarta];
    NSImage *cartaBack = [NSImage imageNamed:@"back.jpg"];
    aRect.size.height = aRect.size.width = DIMENSION;
    aRect.origin.x = funcRect.origin.x + random() % (int)(funcRect.size.width - DIMENSION);
    aRect.origin.x = funcRect.origin.x + random() % (int)(funcRect.size.height - DIMENSION);
    
    newCard = [[Card alloc]initWithCartaDelante:cartaDelante withCartaAtras:cartaBack withRect:aRect withImageName:nombreCarta];
    return newCard;
}

-(void)dibujarCarta:(NSRect)bounds withGraphicsContext:(NSGraphicsContext *)ctx
{
    [ctx saveGraphicsState];
    tf = [NSAffineTransform transform];
    [tf translateXBy:bounds.size.width/2 yBy:bounds.size.height/2];
    [tf scaleXBy:bounds.size.width/funcRect.size.width
             yBy:bounds.size.height/funcRect.size.height];
    [tf concat];
    [rectPath removeAllPoints];
    [rectPath appendBezierPathWithRect:rectangulo];
    [cartaAtras drawInRect:rectangulo];
    
    [ctx restoreGraphicsState];
}

-(BOOL)mouseEvent:(NSPoint)point
       withBounds:(NSRect)bounds
withGraphicsContext:(NSGraphicsContext *)ctx
{
    BOOL encontrado = NO;;
    [ctx saveGraphicsState];
    [tf invert];
    point = [tf transformPoint:point];

    if(NSPointInRect(point, rectangulo)){
        encontrado = YES;
    }
    [ctx restoreGraphicsState];
    return encontrado;
}

-(void)darVueltaCarta
{
    NSImage *aux = cartaDelante;
    [self setCartaDelante: cartaAtras];
    [self setCartaAtras: aux];
    if(abierta)
        [self setAbierta:NO];
    else
        [self setAbierta:YES];
}



@end
