//
//  Card.h
//  GwentGame
//
//  Created by Adrian on 20/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import <Cocoa/Cocoa.h>

NS_ASSUME_NONNULL_BEGIN

@interface Card : NSObject{
    NSImage *cartaDelante;
    NSImage *cartaAtras;
    NSRect rectangulo;
    NSBezierPath *rectPath;
    NSString *imageName;
    NSAffineTransform *tf;
    BOOL abierta;
}

@property (nonatomic, copy) NSImage *cartaDelante;
@property (nonatomic, copy) NSImage *cartaAtras;
@property (nonatomic) NSRect rectangulo;
@property (nonatomic, copy) NSString *imageName;
@property (nonatomic) BOOL abierta;


-(id) initWithCartaDelante: (NSImage *) cartaDelante
            withCartaAtras: (NSImage *) cartaAtras
                  withRect: (NSRect) rect
             withImageName: (NSString *) imageName;

-(void)dibujarCarta: (NSRect)bounds
withGraphicsContext:(NSGraphicsContext *)ctx;
+(id)randomCard;
+(id)copyCardWithOtherOrigin: (NSString *) nombreCarta;
-(BOOL)mouseEvent:(NSPoint)point
       withBounds: (NSRect)bounds
withGraphicsContext:(NSGraphicsContext *)ctx;
-(void)darVueltaCarta;

@end

NS_ASSUME_NONNULL_END
